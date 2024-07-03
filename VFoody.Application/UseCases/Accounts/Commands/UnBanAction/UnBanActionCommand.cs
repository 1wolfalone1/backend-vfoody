using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UnBanAction;

public class UnBanActionCommand : ICommand<Result>
{
    public int AccountId { get; set; }
    public string Reason { get; set; }
}