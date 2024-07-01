using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.DetailPromotionOfShopOwner;

public class GetPromotionShopDetailHandler : IQueryHandler<GetPromotionShopDetailQuery, Result>
{
    private readonly IShopPromotionRepository _shopPromotionRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;

    public GetPromotionShopDetailHandler(
        IShopPromotionRepository shopPromotionRepository, ICurrentPrincipalService currentPrincipalService,
        IShopRepository shopRepository, IMapper mapper)
    {
        _shopPromotionRepository = shopPromotionRepository;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetPromotionShopDetailQuery request, CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        var shopPromotion = _shopPromotionRepository.Get().FirstOrDefault(
            sp => sp.Id == request.Id && sp.ShopId == shop.Id && sp.Status != (int)PromotionStatus.Delete
        );
        if (shopPromotion == null)
        {
            throw new InvalidBusinessException("Không tìm thấy mã giảm giá với id " + request.Id);
        }
        return Result.Success(_mapper.Map<ShopPromotionResponse>(shopPromotion));
    }
}