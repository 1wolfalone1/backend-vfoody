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

    public async Task<Product?> GetProductByIdAndShopId(int productId, int shopId)
    {
        return await DbSet.Where(product =>
                product.Id == productId
                && product.ShopId == shopId
                && product.Status != (int)ProductStatus.Delete
            )
            .SingleOrDefaultAsync();
    }

    public Product? GetIncludeProductCategoryByIdAndShopId(int productId, int shopId)
    {
        return DbSet
            .Where(p =>
                p.Id == productId
                && p.ShopId == shopId
                && p.Status != (int)ProductStatus.Delete
            )
            .Include(p => p.ProductCategories)
            .SingleOrDefault();
    }

    public Product? GetProductDetailCustomer(int productId)
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

    public int CountTotalActiveProductByShopId(int shopId)
    {
        return DbSet
            .Count(p => p.ShopId == shopId && p.Status == (int)ProductStatus.Active);
    }

    public List<Product> GetTopProductByShopId(int shopId, int pageNum, int pageSize)
    {
        return DbSet
            .Where(p => p.ShopId == shopId && p.Status == (int)ProductStatus.Active)
            .OrderByDescending(p => p.TotalOrder)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public async Task<List<Product>> GetListProductInCard(int[] requestProductIds)
    {
        return await this.DbSet.Where(p => requestProductIds.Contains(p.Id) && p.Status != (int)ProductStatus.Delete)
            .ToListAsync();
    }

    public async Task<List<Product>> GetListProductByShopId(int id, int pageNum, int pageSize)
    {
        return await this.DbSet.Where(p => p.ShopId == id && p.Status != (int)ProductStatus.Delete)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public int CountTotalProductByShopId(int id)
    {
        return DbSet
            .Count(p => p.ShopId == id && p.Status != (int)ProductStatus.Delete);
    }

    public Product? GetProductDetailShopOwner(int productId)
    {
        return DbSet
            .Include(p => p.Questions.Where(q =>
                q.Status == (int)QuestionStatus.Active || q.Status == (int)QuestionStatus.UnActive))
            .ThenInclude(q =>
                q.Options.Where(o => o.Status == (int)OptionStatus.Active || o.Status == (int)OptionStatus.UnActive))
            .FirstOrDefault(
                p => p.Id == productId
                     && (p.Status == (int)ProductStatus.Active || p.Status == (int)ProductStatus.UnActive));
    }
}