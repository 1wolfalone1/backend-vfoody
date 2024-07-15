using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.AdminApproveShopWithdrawal;

public class AdminApproveShopWithdrawalCommand : ICommand<Result>
{
    public int ShopId { get; set; }
    public int RequestId { get; set; }
}