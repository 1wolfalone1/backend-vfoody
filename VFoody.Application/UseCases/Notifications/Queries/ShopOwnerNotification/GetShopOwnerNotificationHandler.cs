using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Notifications.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Notifications.Queries.ShopOwnerNotification;

public class GetShopOwnerNotificationHandler : IQueryHandler<GetShopOwnerNotificationQuery, Result>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetShopOwnerNotificationHandler> _logger;

    public GetShopOwnerNotificationHandler(
        INotificationRepository notificationRepository, ICurrentPrincipalService currentPrincipalService,
        IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetShopOwnerNotificationHandler> logger
    )
    {
        _notificationRepository = notificationRepository;
        _currentPrincipalService = currentPrincipalService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetShopOwnerNotificationQuery request, CancellationToken cancellationToken)
    {
        var notificationResult =
            _notificationRepository.GetShopNotifications(
                request.PageIndex, request.PageSize, _currentPrincipalService.CurrentPrincipalId!.Value
            );
        var listNotificationResponse = notificationResult.notifications
            .Select(n => _mapper.Map<NotificationResponse>(n)).ToList();
        var result = new PaginationResponse<NotificationResponse>(listNotificationResponse, request.PageIndex,
            request.PageSize, notificationResult.totalItems);
        await UpdateReadNotification(notificationResult.notifications);
        return Result.Success(result);
    }

    private async Task UpdateReadNotification(List<Notification> notifications)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            notifications.ForEach(x => { x.Readed = 1; });

            this._notificationRepository.UpdateRange(notifications);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            throw;
        }
    }
}