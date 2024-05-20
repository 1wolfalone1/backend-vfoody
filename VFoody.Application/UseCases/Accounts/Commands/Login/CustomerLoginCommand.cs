using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands;

public class CustomerLoginCommand : ICommand<Result>
{
    public AccountLoginRequest AccountLogin { get; set; }
}