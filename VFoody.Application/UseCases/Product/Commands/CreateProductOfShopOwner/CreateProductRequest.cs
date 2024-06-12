using Microsoft.AspNetCore.Http;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;

public sealed record CreateProductRequest(
    IFormFile File,
    string Name,
    string Description,
    float Price,
    List<int> CategoryIds);