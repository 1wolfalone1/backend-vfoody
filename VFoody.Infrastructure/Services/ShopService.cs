using ArtHubBO.Payload;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Infrastructure.Services;

public class ShopService : IShopService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IDapperService dapperService;

    public ShopService(IUnitOfWork unitOfWork, IDapperService dapperService)
    {
        this.unitOfWork = unitOfWork;
        this.dapperService = dapperService;
    }

    private class ShopQueryDTO : Shop {
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }

    public async Task<PageResult<Shop>> GetTopFavouriteShop(int pageIndex, int pageSize)
    {
        try
        {
            var list = await this.dapperService.SelectAsync<ShopQueryDTO>(QueryName.SelectTopFavouriteShop, new
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            });

            var result = new PageResult<Shop>
            {
                PageData = list.Select(item => item as Shop).ToList(),
                PageInfo = new PageInfo
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalPages = list.First().TotalPages,
                    TotalItems = list.First().TotalItems,
                }
            };

            return result;
        }
        catch (Exception e)
        {           
            return null;
        }
    }
}