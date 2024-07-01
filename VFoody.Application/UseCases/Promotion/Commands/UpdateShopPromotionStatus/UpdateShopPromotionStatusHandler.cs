using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.UpdateShopPromotionStatus;

public class UpdateShopPromotionStatusHandler : ICommandHandler<UpdateShopPromotionStatusCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShopPromotionRepository _shopPromotionRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly ILogger<UpdateShopPromotionStatusHandler> _logger;

    public UpdateShopPromotionStatusHandler(
        IUnitOfWork unitOfWork, IShopPromotionRepository shopPromotionRepository,
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository,
        ILogger<UpdateShopPromotionStatusHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _shopPromotionRepository = shopPromotionRepository;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(UpdateShopPromotionStatusCommand request,
        CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        var shopPromotion = _shopPromotionRepository.Get().SingleOrDefault(
            sp => sp.Id == request.Id && sp.ShopId == shop.Id && sp.Status != (int)PromotionStatus.Delete
        );
        if (shopPromotion == null)
        {
            throw new InvalidBusinessException("Không tìm thấy mã giảm giá với id " + request.Id);
        }

        shopPromotion.Status = (int)request.Status;
        try
        {
            await _unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            _shopPromotionRepository.Update(shopPromotion);
            await _unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Success("Cập nhật status thành công.");
        }
        catch (Exception e)
        {
            //Rollback when exception
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }
}