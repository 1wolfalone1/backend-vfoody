using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetProductByIdAndShopId(int productId, int shopId);
    Product? GetProductIncludeProductCategoryByIdAndShopId(int productId, int shopId);
    Product? GetProductDetail(int productId);
    List<Product> GetShopProduct(int shopId, int pageNum, int pageSize);
    int CountTotalActiveProductByShopId(int shopId);
    List<Product> GetTopProductByShopId(int shopId, int pageNum, int pageSize);
    Task<List<Product>> GetListProductInCard(int[] requestProductIds);
    Task<List<Product>> GetListProductByShopId(int id, int pageNum, int pageSize);
    int CountTotalProductByShopId(int id);
}
