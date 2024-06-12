using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;

public class CreateProductCommand : ICommand<Result>
{
    public IFormFile File { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public List<int> CategoryIds { get; set; }
}