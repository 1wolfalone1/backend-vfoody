using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;

public sealed record VerifyCodeForgotPasswordCommand(string Email, int Code) : ICommand<Result>;