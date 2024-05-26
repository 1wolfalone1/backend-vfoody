using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email, int Code, string NewPassword) : ICommand<Result>;