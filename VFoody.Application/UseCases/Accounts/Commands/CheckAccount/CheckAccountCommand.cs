using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.CheckAccount;

public sealed record CheckAccountCommand(string PhoneNumber, string Email) : ICommand<Result>;