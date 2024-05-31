using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.Register;

public sealed record CustomerRegisterCommand(string PhoneNumber, string Email, string Password) : ICommand<Result>;