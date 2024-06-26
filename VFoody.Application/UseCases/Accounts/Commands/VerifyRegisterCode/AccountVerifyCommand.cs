using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.VerifyRegisterCode;

public sealed record AccountVerifyCommand(string Email, int Code) : ICommand<Result>;