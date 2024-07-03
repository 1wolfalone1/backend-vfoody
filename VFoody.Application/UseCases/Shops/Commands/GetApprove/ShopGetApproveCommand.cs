using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.GetApprove;

public class ShopGetApproveCommand : ICommand<Result>
{
    public int ShopId { get; set; }
}