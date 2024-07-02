using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopDetail;

public class GetShopDetailHandler : IQueryHandler<GetShopDetailQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetShopDetailHandler> _logger;
    private readonly IMapper _mapper;

    public GetShopDetailHandler(IDapperService dapperService, ILogger<GetShopDetailHandler> logger, IMapper mapper)
    {
        _dapperService = dapperService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetShopDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var parameter = new
            {
                DeleteStatus = (int)ShopStatus.Delete,
                OrderSuccessful = (int)OrderStatus.Successful,
                ShopId = request.ShopId,
            };

            var shop = await _dapperService.SingleOrDefaultAsync<ManageShopDetailDto>(
                    QueryName.SelectShopDetail, parameter)
                .ConfigureAwait(false);

            if (shop == null)
            {
                return Result.Failure(new Error("400", "Không tìm thấy shop."));
            }

            return Result.Success(_mapper.Map<ManageShopDetailResponse>(shop));
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal error"));
        }
    }
}