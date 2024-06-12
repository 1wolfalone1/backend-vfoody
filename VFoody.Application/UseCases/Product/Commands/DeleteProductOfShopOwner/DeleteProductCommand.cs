using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.DeleteProductOfShopOwner;

public class DeleteProductCommand : ICommand<Result>
{
    public int Id { get; set; }
}