using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Esf;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Transactions.Commands.PaymentForOrder.PaymentSuccess;

public class PaymentSuccessHandler : ICommandHandler<PaymentSucessCommand, Result>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderHistoryRepository _orderHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseNotificationService _firebaseNotification;
    private readonly IAccountRepository _accountRepository;
    private readonly IShopRepository _shopRepository;
    private readonly ILogger<PaymentSuccessHandler> _logger;
    private readonly INotificationRepository _notificationRepository;

    public PaymentSuccessHandler(ITransactionRepository transactionRepository, ITransactionHistoryRepository transactionHistoryRepository, IUnitOfWork unitOfWork, IOrderRepository orderRepository, IOrderHistoryRepository orderHistoryRepository, IFirebaseNotificationService firebaseNotification, IAccountRepository accountRepository, IShopRepository shopRepository, ILogger<PaymentSuccessHandler> logger, INotificationRepository notificationRepository)
    {
        _transactionRepository = transactionRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _orderHistoryRepository = orderHistoryRepository;
        _firebaseNotification = firebaseNotification;
        _accountRepository = accountRepository;
        _shopRepository = shopRepository;
        _logger = logger;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<Result>> Handle(PaymentSucessCommand request, CancellationToken cancellationToken)
    {
        var order = this._orderRepository.GetById(request.OrderId);
        var transaction = this._transactionRepository.GetById(order.TransactionId);
        var shop = this._shopRepository.GetById(order.ShopId);
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            await this.UpdateOrderAsync(order).ConfigureAwait(false);
            await this.UpdateTransactionAsync(transaction, order.Id, request).ConfigureAwait(false);
            await this.UpdateShopBalance(shop, transaction.Amount);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);

            var account = this._accountRepository.GetById(order.AccountId);
            // Send noti for customer 
            await this.SendNotificationAsync(order.AccountId,
                account.DeviceToken,
                NotificationMessageConstants.Order_Title,
                NotificationMessageConstants.Payment_Order_Success,
                (int)Domain.Enums.Roles.Customer);
            
            // Send noti for shop
            var shopAccount = this._shopRepository.GetAccountByShopId(order.ShopId);
            await this.SendNotificationAsync(shopAccount.Id,
                shopAccount.DeviceToken,
                NotificationMessageConstants.Order_Title,
                NotificationMessageConstants.Order_Successfull_Content,
                (int)Domain.Enums.Roles.Shop);
            
            // Send noti receive money
            await this.SendNotificationAsync(shopAccount.Id,
                shopAccount.DeviceToken,
                NotificationMessageConstants.Shop_Balance_Title,
                string.Format(NotificationMessageConstants.Shop_Balance_Plus_From_Order,
                    transaction.Amount, order.Id),
                (int)Domain.Enums.Roles.Shop);
            return Result.Success("Thanh toán đơn hàng thành công vui lòng quay lại app");
        }
        catch (Exception e)
        {
            var account = this._accountRepository.GetById(order.AccountId);
            await this.SendNotificationAsync(order.AccountId,
                account.DeviceToken,
                NotificationMessageConstants.Order_Title,
                NotificationMessageConstants.Payment_Order_Fail,
                (int)Domain.Enums.Roles.Customer);
            this._unitOfWork.RollbackTransaction();
            throw;
        }
    }

    private async Task UpdateTransactionAsync(Transaction transaction, int orderId, PaymentSucessCommand request)
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
        transaction.Status = (int)TransactionStatus.PaidSuccess;
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
        order.Status = (int)OrderStatus.Successful;
        this._orderRepository.Update(order);
    }

    private async Task UpdateShopBalance(Domain.Entities.Shop shop, float amount)
    {
        shop.Balance += amount;
        this._shopRepository.Update(shop);
    }
    
    private async Task SendNotificationAsync(int accountId, string deviceToken, string title, string content, int role)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            this._firebaseNotification.SendNotification(deviceToken, title, content);
            Notification noti = new Notification()
            {
                AccountId = accountId,
                Readed = 0,
                Title = title,
                Content = content,
                ImageUrl = string.Empty,
                RoleId = role,
            };
            await this._notificationRepository.AddAsync(noti).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
        }
    }
}