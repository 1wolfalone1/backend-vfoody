using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ManageShop;

public class GetAllShopHandler : IQueryHandler<GetAllShopQuery, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;

    public GetAllShopHandler(IShopRepository shopRepository, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetAllShopQuery request, CancellationToken cancellationToken)
    {
        var totalShops = _shopRepository.CountAll();
        var shops = _shopRepository.GetAllShopIncludeAddress(
            request.PageIndex, request.PageSize
        );
        var result = new PaginationResponse<ManageShopResponse>(_mapper.Map<List<ManageShopResponse>>(shops),
            request.PageIndex, request.PageSize, totalShops);
        return Result.Success(result);
    }
}