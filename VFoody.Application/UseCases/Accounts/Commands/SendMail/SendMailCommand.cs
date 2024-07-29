using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.SendMail;

public sealed record SendMailCommand() : ICommand<Result>;