using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.RegisterVerifyFirebase;

public sealed record RegisterFireBaseCommand(string Email, string PhoneNumber, string Password, string Token) : ICommand<Result>;