using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class ShopBalanceHistoryRepository :  BaseRepository<ShopBalanceHistory>, IShopBalanceHistoryRepository
{
    public ShopBalanceHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public (List<ShopBalanceHistory> ListShopBalance, int TotalItem) GetShopBalanceHistory(int requestPageIndex,
        int requestPageSize, int shopId, int transactionType)
    {
        var query = this.DbSet.AsQueryable();
        query = query.Where(sh => sh.ShopId == shopId);
        if (transactionType != 0)
        {
            query = query.Where(sh => sh.TransactionType == (int)transactionType);
        }

        var totalItem = query.Count();
        var listShop = query.Skip((requestPageIndex - 1) * requestPageSize)
            .Take(requestPageSize)
            .OrderByDescending(sh => sh.UpdatedDate)
            .ToList();

        return (listShop, totalItem);
    }
}