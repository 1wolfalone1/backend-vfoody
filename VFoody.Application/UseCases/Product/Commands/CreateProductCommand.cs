using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands;

public class CreateProductCommand : ICommand<Result>
{
    public CreateProductRequest CreateProductRequest { get; set; }
}