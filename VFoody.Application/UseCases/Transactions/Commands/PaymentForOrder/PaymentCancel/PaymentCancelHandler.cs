using Newtonsoft.Json;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Transactions.Commands.PaymentForOrder.PaymentSuccess;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Transactions.Commands.PaymentForOrder.PaymentCancel;

public class PaymentCancelHandler : ICommandHandler<PaymenCancelCommand, Result>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderHistoryRepository _orderHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseNotificationService _firebaseNotification;
    private readonly IAccountRepository _accountRepository;

    public PaymentCancelHandler(ITransactionRepository transactionRepository, ITransactionHistoryRepository transactionHistoryRepository, IUnitOfWork unitOfWork, IOrderRepository orderRepository, IOrderHistoryRepository orderHistoryRepository, IFirebaseNotificationService firebaseNotification, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _orderHistoryRepository = orderHistoryRepository;
        _firebaseNotification = firebaseNotification;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Result>> Handle(PaymenCancelCommand request, CancellationToken cancellationToken)
    {
        var order = this._orderRepository.GetById(request.OrderId);
        var transaction = this._transactionRepository.GetById(order.TransactionId);
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            await this.UpdateOrderAsync(order).ConfigureAwait(false);
            await this.UpdateTransactionAsync(transaction, order.Id, request).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);

            var account = this._accountRepository.GetById(order.AccountId);
            this._firebaseNotification.SendNotification(account.DeviceToken,
                NotificationMessageConstants.Order_Title,
                NotificationMessageConstants.Payment_Order_Success);
            return Result.Success("Hủy thanh toán đơn hàng thành công! Vui lòng quay lại app");
        }
        catch (Exception e)
        {
            var account = this._accountRepository.GetById(order.AccountId);
            this._firebaseNotification.SendNotification(account.DeviceToken,
                NotificationMessageConstants.Order_Title,
                NotificationMessageConstants.Payment_Order_Fail);
            this._unitOfWork.RollbackTransaction();
            throw;
        }
    }

    private async Task UpdateTransactionAsync(Transaction transaction, int orderId, PaymenCancelCommand request)
    {
        var transactionHistory = new TransactionHistory()
        {
            OrderId = orderId,
            TransactionId = transaction.Id,
            Amount = transaction.Amount,
            Status = transaction.Status,
            TransactionType = transaction.TransactionType,
            PaymentThirdpartyId = transaction.PaymentThirdpartyId,
            PaymentThirdpartyContent = transaction.PaymentThirdpartyContent,
            CreatedDate = transaction.CreatedDate,
            UpdatedDate = transaction.UpdatedDate
        };

        await this._transactionHistoryRepository.AddAsync(transactionHistory).ConfigureAwait(false);
        transaction.Status = (int)TransactionStatus.PaidCancel;
        transaction.PaymentThirdpartyContent = JsonConvert.SerializeObject(request);
        this._transactionRepository.Update(transaction);
    }
    
    private async Task UpdateOrderAsync(Order order)
    {
        var historyOrder = new OrderHistory()
        {
            OrderId = order.Id,
            Status = order.Status,
            TransactionId = order.TransactionId,
            ShippingFee = order.ShippingFee,
            Note = order.Note,
            ShopPromotionId = order.ShopPromotionId,
            PlatformPromotionId = order.PlatformPromotionId,
            PersonalPromotionId = order.PersonalPromotionId,
            ShopId = order.ShopId,
            AccountId = order.AccountId,
            BuildingId = order.BuildingId,
            TotalPrice = order.TotalPrice,
            TotalPromotion = order.TotalPromotion,
            ChargeFee = order.ChargeFee,
            FullName = order.FullName,
            PhoneNumber = order.PhoneNumber,
            CreatedDate = order.CreatedDate,
            UpdatedDate = order.UpdatedDate
        };

        await this._orderHistoryRepository.AddAsync(historyOrder).ConfigureAwait(false);
    }
}