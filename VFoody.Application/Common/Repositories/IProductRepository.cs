using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetProductByIdAndShopId(int productId, int shopId);
    Product? GetIncludeProductCategoryByIdAndShopId(int productId, int shopId);
    Product? GetProductDetailCustomer(int productId);
    List<Product> GetShopProduct(int shopId, int pageNum, int pageSize);
    int CountTotalActiveProductByShopId(int shopId);
    List<Product> GetTopProductByShopId(int shopId, int pageNum, int pageSize);
    Task<List<Product>> GetListProductInCard(int[] requestProductIds);
    Task<List<Product>> GetListProductByShopId(int id, int? status, int pageNum, int pageSize);
    int CountTotalProductByShopId(int id);
    Product? GetProductDetailShopOwner(int productId, int shopId);
}
