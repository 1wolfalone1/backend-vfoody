using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Firebases.Commands.CreateUser;

public class CreateUserCommand : ICommand<Result>
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string DisplayName { get; set; }
    public string AvatarUrl { get; set; }
}