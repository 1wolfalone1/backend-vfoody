using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class ShopWithdrawalRequestRepository : BaseRepository<ShopWithdrawalRequest>, IShopWithdrawalRequestRepository
{
    public ShopWithdrawalRequestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public (List<ShopWithdrawalRequest> ListWithdrawals, int TotalItem) GetShopWithdrawalHistory(
        int requestPageSize, int requestPageIndex, int shopId, int status)
    {
        var query = this.DbSet.AsQueryable();
        query = query.Where(sh => sh.ShopId == shopId);
        if (status != 0)
        {
            query = query.Where(sh => sh.Status == status);
        }

        var totalItem = query.Count();

        var listResult = query.Skip((requestPageIndex - 1) * requestPageSize)
            .Take(requestPageSize)
            .OrderByDescending(sh => sh.UpdatedDate)
            .ToList();

        return (listResult, totalItem);
    }
}