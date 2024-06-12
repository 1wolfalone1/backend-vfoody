using Microsoft.AspNetCore.Http;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;

public sealed record UpdateProductRequest(
    int Id,
    IFormFile? File,
    string? Name,
    string? Description,
    float? Price,
    List<int>? CategoryIds);