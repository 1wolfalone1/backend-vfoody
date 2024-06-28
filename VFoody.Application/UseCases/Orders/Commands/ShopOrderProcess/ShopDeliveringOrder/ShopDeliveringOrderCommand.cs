using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;
using ICommand = System.Windows.Input.ICommand;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopDeliveringOrder;

public class ShopDeliveringOrderCommand : ICommand<Result>
{
    public int OrderId { get; set; }
}