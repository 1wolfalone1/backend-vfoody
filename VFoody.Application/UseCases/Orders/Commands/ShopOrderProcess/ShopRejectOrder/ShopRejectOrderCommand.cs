using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopRejectOrder;

public class ShopRejectOrderCommand : ICommand<Result>
{
    public int OrderId { get; set; }
    public string Reason { get; set; }
}