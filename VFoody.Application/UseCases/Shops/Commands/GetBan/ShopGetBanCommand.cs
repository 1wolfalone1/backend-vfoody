using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.GetBan;

public class ShopGetBanCommand : ICommand<Result>
{
    public int ShopId { get; set; }
    public string Reason { get; set; }
}