using Org.BouncyCastle.Ocsp;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.BanAction;

public class BanActionCommand : ICommand<Result>
{
    public int AccountId { get; set; }   
    public string Reason { get; set; }
}