using Microsoft.AspNetCore.Http;

namespace VFoody.Application.UseCases.Product.Commands;

public sealed record CreateProductRequest(IFormFile File);