using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopRejectOrder;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopCancelOrder;

public class ShopCancelOrderHandler : ICommandHandler<ShopCancelOrderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IShopRepository _shopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IAccountRepository _accountRepository;
    private readonly IFirebaseNotificationService _firebaseNotification;
    private readonly IOrderHistoryRepository _orderHistoryRepository;
    private readonly ILogger<ShopRejectOrderHandler> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly IFirebaseFirestoreService _firebaseFirestoreService;

    public ShopCancelOrderHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IShopRepository shopRepository, ICurrentPrincipalService currentPrincipalService, IAccountRepository accountRepository, IFirebaseNotificationService firebaseNotification, IOrderHistoryRepository orderHistoryRepository, ILogger<ShopRejectOrderHandler> logger, INotificationRepository notificationRepository, IFirebaseFirestoreService firebaseFirestoreService)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _shopRepository = shopRepository;
        _currentPrincipalService = currentPrincipalService;
        _accountRepository = accountRepository;
        _firebaseNotification = firebaseNotification;
        _orderHistoryRepository = orderHistoryRepository;
        _logger = logger;
        _notificationRepository = notificationRepository;
        _firebaseFirestoreService = firebaseFirestoreService;
    }

    public async Task<Result<Result>> Handle(ShopCancelOrderCommand request, CancellationToken cancellationToken)
    {
        var shop = await this._shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId
            .Value).ConfigureAwait(false);
        var order = await this._orderRepository.GetOrderOfShopByIdAsync(request.OrderId, shop.Id);

        if (order == default)
            throw new InvalidBusinessException($"Đơn hàng VFD{request.OrderId} không thuộc quyền quản lí của cửa hàng");

        if (order.Status != (int)OrderStatus.Confirmed)
            throw new InvalidBusinessException($"Đơn hàng VFD{request.OrderId} đang trong trạng thái không thể hủy");
        
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            await this.UpdateOrderAsync(order, request.Reason).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            var customerAccount = this._accountRepository.GetById(order.AccountId);
            var notificationMessage = string.Format(NotificationMessageConstants.Order_Shop_Cancel_Content, order.Id);
            await this.SendNotificationAsync(shop.LogoUrl
                ,order.AccountId,
                customerAccount.DeviceToken,
                NotificationMessageConstants.Order_Title,
                notificationMessage,
                (int)Domain.Enums.Roles.Customer);
            await this._firebaseFirestoreService.AddNewNotifyCollectionToUser(customerAccount.Email,
                FirebaseStoreConstants.Order_Type,
                order.Status, notificationMessage);
            return Result.Success($"Chuyển sang trạng thái hủy đơn hàng VFD{request.OrderId} thành công");
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            throw;
        }
    }
    
    private async Task UpdateOrderAsync(Order order, string reason)
    {
        var historyOrder = new OrderHistory()
        {
            Reason = "Cửa hàng hủy với lí do: " + reason,
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
    
    private async Task SendNotificationAsync(string imageUrl, int accountId, string deviceToken, string title, string content, int role)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            this._firebaseNotification.SendNotification(deviceToken, title, content, imageUrl);
            Notification noti = new Notification()
            {
                AccountId = accountId,
                Readed = 0,
                Title = title,
                Content = content,
                ImageUrl = imageUrl,
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