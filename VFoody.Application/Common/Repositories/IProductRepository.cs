using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Product? GetProductDetail(int productId);
    List<Product> GetShopProduct(int shopId, int pageNum, int pageSize);
    List<Product> GetTopProductByShopId(int shopId, int pageNum, int pageSize);
}
