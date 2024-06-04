using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.CustomerCancels;

public class CustomerCancelCommand : ICommand<Result>
{
    public int Id { get; set; }
}