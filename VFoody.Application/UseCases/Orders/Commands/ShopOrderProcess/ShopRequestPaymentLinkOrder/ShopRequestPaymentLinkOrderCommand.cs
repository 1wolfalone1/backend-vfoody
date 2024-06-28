using System.Windows.Input;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.ShopOrderProcess.ShopRequestPaidOrder;

public class ShopRequestPaymentLinkOrderCommand : ICommand<Result>
{
    public int OrderId { get; set; }
}