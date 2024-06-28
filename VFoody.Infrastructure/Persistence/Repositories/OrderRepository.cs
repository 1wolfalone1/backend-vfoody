using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<bool> CheckInOrderByProductId(int id)
    {
        int[] orderStatusDone = { (int)OrderStatus.Cancelled, (int)OrderStatus.Successful, (int)OrderStatus.Fail };

        // Use AnyAsync for asynchronous execution and proper LINQ query
        return await DbSet.Include(o => o.OrderDetails)
            .Where(o => o.OrderDetails.Any(od => od.ProductId == id) && !orderStatusDone.Contains(o.Status))
            .AnyAsync();
    }

    public async Task<Order> GetOrderOfShopById(int orderId, int shopId)
    {
        return await this.DbSet.Include(x => x.Building)
            .SingleOrDefaultAsync(x =>  x.Id == orderId && x.ShopId == shopId).ConfigureAwait(false);
    }
}