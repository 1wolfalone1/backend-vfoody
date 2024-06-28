using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopFailOrder;

public class ShopFailOrderCommand : ICommand<Result>
{
    public int OrderId { get; set; }
    public string Reason { get; set; }
}