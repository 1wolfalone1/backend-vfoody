using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.GetUnBan;

public class ShopGetUnBanCommand : ICommand<Result>
{
    public int ShopId { get; set; }
    public string Reason { get; set; }
}