using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopRequestPaidOrder;

public class ShopRequestPaymentLinkOrderHandler : ICommandHandler<ShopRequestPaymentLinkOrderCommand, Result>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShopRepository _shopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IDapperService _dapperService;
    private readonly IPayOSService _payOsService;
    private readonly IConfiguration _configuration;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ShopRequestPaymentLinkOrderHandler(IOrderRepository orderRepository, IShopRepository shopRepository, ICurrentPrincipalService currentPrincipalService, IDapperService dapperService, IPayOSService payOsService, IConfiguration configuration, ITransactionRepository transactionRepository, ITransactionHistoryRepository transactionHistoryRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _shopRepository = shopRepository;
        _currentPrincipalService = currentPrincipalService;
        _dapperService = dapperService;
        _payOsService = payOsService;
        _configuration = configuration;
        _transactionRepository = transactionRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(ShopRequestPaymentLinkOrderCommand request, CancellationToken cancellationToken)
    {
        var shop = await this._shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId!.Value);
        var order = await this._orderRepository.GetOrderOfShopByIdAsync(request.OrderId, shop.Id).ConfigureAwait(false);
        if (order == default)
            throw new InvalidBusinessException($"Shop không có quyền cập nhật order id: {request.OrderId}");

        if (order.Status != (int)OrderStatus.Delivering)
            throw new InvalidBusinessException($"Đơn hàng đang không ở trạng thái có thể tạo link thanh toán");

        var listItemPayment = await this._dapperService.SelectAsync<ItemPayment>(
            QueryName.SelectItemPaymentInOrder,
            new
            {
                OrderId = request.OrderId
            }).ConfigureAwait(false);

        // Create Payment Link
        int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        string cancelUrl = this._configuration["BASE_URL"] + $"api/v1/transaction/{order.Id}/cancel";
        string successUrl = this._configuration["BASE_URL"] + $"api/v1/transaction/{order.Id}/success";
        var createPayment = await this._payOsService.CheckOut(listItemPayment.ToList(),
            orderCode,
            (order.TotalPrice + order.ShippingFee - order.TotalPromotion),
            order,
            cancelUrl,
            successUrl
        ).ConfigureAwait(false);
        var transaction = this._transactionRepository.GetById(order.TransactionId);
        var result = await this.UpdateTransactionStatusAsync(transaction, order.Id, createPayment).ConfigureAwait(false);
        return Result.Success(new
        {
            CheckOutLink = createPayment.CheckoutUrl
        });
    }

    private async Task<bool> UpdateTransactionStatusAsync(Transaction transaction, int orderId, CreatePayment createPayment)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
           // Create history
            var transactionHistory = new TransactionHistory()
            {
                OrderId = orderId,
                TransactionId = transaction.Id,
                Amount = transaction.Amount,
                Status = transaction.Status,
                TransactionType = transaction.TransactionType,
                PaymentThirdpartyId = transaction.PaymentThirdpartyId,
                PaymentThirdpartyContent = transaction.PaymentThirdpartyContent
            };
            await this._transactionHistoryRepository.AddAsync(transactionHistory);

            // Update current transaction
            transaction.TransactionType = (int)TransactionTypes.PayOS;
            transaction.PaymentThirdpartyId = createPayment.PaymentLinkId;
            transaction.PaymentThirdpartyContent = JsonConvert.SerializeObject(createPayment);
            transaction.Status = (int)TransactionStatus.Pending;
            this._transactionRepository.Update(transaction);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return true;
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            throw;
        }
    }
}