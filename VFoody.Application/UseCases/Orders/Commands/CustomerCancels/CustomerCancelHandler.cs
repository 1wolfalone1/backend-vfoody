using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.CustomerCancels;

public class CustomerCancelHandler : ICommandHandler<CustomerCancelCommand,Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CustomerCancelHandler> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly IFirebaseFirestoreService _firebaseFirestoreService;
    private readonly IFirebaseNotificationService _firebaseNotification;
    private readonly IOrderHistoryRepository _orderHistoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IShopRepository _shopRepository;

    public CustomerCancelHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, ILogger<CustomerCancelHandler> logger, INotificationRepository notificationRepository, IFirebaseFirestoreService firebaseFirestoreService, IFirebaseNotificationService firebaseNotification, IOrderHistoryRepository orderHistoryRepository, IAccountRepository accountRepository, IShopRepository shopRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _logger = logger;
        _notificationRepository = notificationRepository;
        _firebaseFirestoreService = firebaseFirestoreService;
        _firebaseNotification = firebaseNotification;
        _orderHistoryRepository = orderHistoryRepository;
        _accountRepository = accountRepository;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(CustomerCancelCommand request, CancellationToken cancellationToken)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var order = this._orderRepository.Get(predicate: a => a.Id == request.Id
                                                                  && a.Status == (int)OrderStatus.Pending).FirstOrDefault();
            if (order == null)
                throw new InvalidBusinessException($"Đơn hàng đang không trong trạng thái hủy");

            await this.UpdateOrderAsync(order, request.Reason).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            
            // Send noti for user
            var customerAccount = this._accountRepository.GetById(order.AccountId);
            var notificationMessage = string.Format(NotificationMessageConstants.Order_Cancel_Content, order.Id);
            await this.SendNotificationAsync(order.AccountId,
                customerAccount.DeviceToken,
                NotificationMessageConstants.Order_Title,
                notificationMessage,
                (int)Domain.Enums.Roles.Customer);
            await this._firebaseFirestoreService.AddNewNotifyCollectionToUser(customerAccount.Email,
                FirebaseStoreConstants.Order_Type,
                order.Status, notificationMessage);
            
            // Send noti for shop
            var shopAccount = this._shopRepository.GetAccountByShopId(order.ShopId);
            await this.SendNotificationAsync(shopAccount.Id,
                shopAccount.DeviceToken,
                NotificationMessageConstants.Order_Title,
                notificationMessage,
                (int)Domain.Enums.Roles.Shop);
            await this._firebaseFirestoreService.AddNewNotifyCollectionToShop(shopAccount.Email,
                FirebaseStoreConstants.Order_Type,
                order.Status, notificationMessage);

            return Result.Success($"Hủy đơn hàng VFD{order.Id} thành công");
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            this._unitOfWork.RollbackTransaction();
            return Result.Failure(new Error("500", "Hủy order thất bại"));
        }
    }
    
    private async Task UpdateOrderAsync(Order order, string reason)
    {
        var historyOrder = new OrderHistory()
        {
            Reason = "Khách hàng hủy với lí do: " + reason,
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
        order.Status = (int)OrderStatus.Cancelled;
        this._orderRepository.Update(order);
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