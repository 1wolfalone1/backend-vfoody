using Microsoft.AspNetCore.Http;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductImageOfShopOwner;

public sealed record CreateProductImageRequest(IFormFile File);