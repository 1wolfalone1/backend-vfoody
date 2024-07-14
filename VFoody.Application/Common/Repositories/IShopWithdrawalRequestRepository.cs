using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IShopWithdrawalRequestRepository : IBaseRepository<ShopWithdrawalRequest>
{  
    (List<ShopWithdrawalRequest> ListWithdrawals, int TotalItem) GetShopWithdrawalHistory(
        int requestPageSize, int requestPageIndex, int shopId, int stastus);
}