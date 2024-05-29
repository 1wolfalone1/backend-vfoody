using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public Product? GetProductDetail(int productId)
    {
        return DbSet
            .Include(p => p.Questions.Where(q =>
                q.Status == (int)QuestionStatus.Active))
            .ThenInclude(q =>
                q.Options.Where(o => o.Status == (int)OptionStatus.Active || o.Status == (int)OptionStatus.UnActive))
            .FirstOrDefault(
                p => p.Id == productId
                     && p.Status == (int)ProductStatus.Active);
    }

    public List<Product> GetShopProduct(int shopId, int pageNum, int pageSize)
    {
        return DbSet
            .Where(p => p.ShopId == shopId && p.Status == (int)ProductStatus.Active)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}