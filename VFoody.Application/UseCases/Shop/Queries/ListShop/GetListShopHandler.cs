using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ListShop;

public class GetListShopHandler : IQueryHandler<GetListShopQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetListShopHandler> _logger;
    

    public GetListShopHandler(IDapperService dapperService, ILogger<GetListShopHandler> logger)
    {
        _dapperService = dapperService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetListShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await _dapperService.SelectAsync<SelectSimpleShopDTO>(QueryName.SelectShopByIds, new
            {
                ShopIds = request.shopIds
            }).ConfigureAwait(false);

            if (list != null && list.Count() > 0)
            {
                return Result.Success(list);
            }
            
            return Result.Failure(new Error("404", $"Không tìm thấy danh sách shop với ids: {string.Join(", ", request.shopIds)}"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}