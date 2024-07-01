using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Promotion.Commands.CreateShopPromotion;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.UpdateShopPromotion;

public class UpdateShopPromotionHandler : ICommandHandler<UpdateShopPromotionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShopPromotionRepository _shopPromotionRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly ILogger<CreateShopPromotionHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateShopPromotionHandler(
        IUnitOfWork unitOfWork, IShopPromotionRepository shopPromotionRepository,
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository,
        ILogger<CreateShopPromotionHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _shopPromotionRepository = shopPromotionRepository;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(UpdateShopPromotionCommand request, CancellationToken cancellationToken)
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

        if (request.ApplyType == PromotionApplyTypes.Percent)
        {
            if (request.AmountRate <= 0)
            {
                throw new InvalidBusinessException("Phần trăm giảm giá phải lớn hơn 0.");
            }

            if (request.MaximumApplyValue <= 0)
            {
                throw new InvalidBusinessException("Số tiền giảm giá tối đa phải lớn hơn 0.");
            }
            shopPromotion.AmountRate = request.AmountRate;
            shopPromotion.MaximumApplyValue = request.MaximumApplyValue;
            shopPromotion.AmountValue = 0;
            shopPromotion.ApplyType = (int)PromotionApplyTypes.Percent;
        }
        else
        {
            if (request.AmountValue <= 0)
            {
                throw new InvalidBusinessException("Số tiền giảm giá phải lớn hơn 0.");
            }
            shopPromotion.AmountRate = 0;
            shopPromotion.MaximumApplyValue = 0;
            shopPromotion.AmountValue = request.AmountValue;
            shopPromotion.ApplyType = (int)PromotionApplyTypes.Absolute;
        }
        shopPromotion.Title = request.Title;
        shopPromotion.Description = request.Description;
        shopPromotion.MinimumOrderValue = request.MinimumOrderValue;
        shopPromotion.StartDate = request.StartDate;
        shopPromotion.EndDate = request.EndDate;
        shopPromotion.UsageLimit = request.UsageLimit;

        try
        {
            await _unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            _shopPromotionRepository.Update(shopPromotion);
            await _unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Success(_mapper.Map<ShopPromotionResponse>(shopPromotion));
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