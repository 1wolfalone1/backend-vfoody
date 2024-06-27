using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopConfirmOrder;

public class ShopConfirmOrderHadler : ICommandHandler<ShopConfirmOrderCommand, Result>
{
    private readonly IFirebaseNotificationService _firebaseNotification;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ShopConfirmOrderCommand> _logger;

    public ShopConfirmOrderHadler(IFirebaseNotificationService firebaseNotification, ICurrentPrincipalService currentPrincipalService, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IShopRepository shopRepository, IAccountRepository accountRepository, ILogger<ShopConfirmOrderCommand> logger)
    {
        _firebaseNotification = firebaseNotification;
        _currentPrincipalService = currentPrincipalService;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _shopRepository = shopRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(ShopConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var shop = await this._shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId!.Value);
        var order = this._orderRepository.Get(x => x.Id == request.OrderId &&
                                                   x.ShopId == shop.Id).SingleOrDefault();
        if (order == default)
            throw new InvalidBusinessException($"Shop không có quyền cập nhật order id: {request.OrderId}");

        if (order.Status != (int)OrderStatus.Pending)
            throw new InvalidBusinessException($"Đơn hàng đang không ở trạng thái có thể nhận");

        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            order.Status = (int)OrderStatus.Confirmed;
            this._orderRepository.Update(order);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            var customerAccount = this._accountRepository.GetById(order.AccountId);
            this._firebaseNotification.SendNotification(customerAccount.DeviceToken,
                NotificationMessageConstants.Order_Title,
                NotificationMessageConstants.Order_Confirmed_Content);
            return Result.Success($"Nhận đơn hàng VFD{request.OrderId} thành công");
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            throw;
        }

    }
}