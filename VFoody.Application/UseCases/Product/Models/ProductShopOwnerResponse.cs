namespace VFoody.Application.UseCases.Product.Models;

public class ProductShopOwnerResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public int TotalOrder { get; set; }
    public float Price { get; set; }
    public int Status { get; set; }
}