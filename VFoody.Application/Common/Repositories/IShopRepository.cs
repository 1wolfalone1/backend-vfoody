using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IShopRepository : IBaseRepository<Shop>
{
    bool IsActiveShop(int shopId);
    int CountAll();
    List<Shop> GetAllShopIncludeAddressAccount(int pageNum, int pageSize);
    Shop? GetInfoByShopIdAndStatusIn(int shopId, int[] statusList);
    Task<Shop> GetShopByAccountId(int id);

    Account GetAccountByShopId(int shopId);
}
