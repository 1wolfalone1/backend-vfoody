using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.ReVerify;

public sealed record AccountReVerifyCommand(string Email) : ICommand<Result>;