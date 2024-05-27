using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.SendCode;

public sealed record AccountSendCodeCommand(string Email, int VerifyType) : ICommand<Result>;