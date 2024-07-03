using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductStatusOfShopOwner;

public class UpdateProductStatusCommand : ICommand<Result>
{
    public int Id { get; set; }
    public ProductStatus Status { get; set; }
}