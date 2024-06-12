using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;

public class UpdateProductCommand : ICommand<Result>
{
    public int Id { get; set; }
    public IFormFile? File { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public List<int>? CategoryIds { get; set; }
}