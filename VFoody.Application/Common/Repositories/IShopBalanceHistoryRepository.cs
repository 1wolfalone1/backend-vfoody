using VFoody.Domain.Entities;
using VFoody.Domain.Enums;

namespace VFoody.Application.Common.Repositories;

public interface IShopBalanceHistoryRepository : IBaseRepository<ShopBalanceHistory>
{
    (List<ShopBalanceHistory> ListShopBalance, int TotalItem) GetShopBalanceHistory(int requestPageIndex,
        int requestPageSize,
        int shopId, int transactionType);
}