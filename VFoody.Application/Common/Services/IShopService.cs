using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.Common.Services
{
    public interface IShopService
    {
        Task<PageResult<Shop>> GetTopFavouriteShop(int pageIndex, int pageSize);
    }
}
