using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.AdminRejectShopWithdrawal;

public class AdminRejectShopWithdrawalCommand : ICommand<Result>
{
    public int ShopId { get; set; }
    public int RequestId { get; set; }
    public string Reason { get; set; }
}