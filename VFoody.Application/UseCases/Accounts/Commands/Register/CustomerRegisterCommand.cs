using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands;

public sealed record CustomerRegisterCommand(string FirstName, string LastName, string Email, string Password) : ICommand<Result>;