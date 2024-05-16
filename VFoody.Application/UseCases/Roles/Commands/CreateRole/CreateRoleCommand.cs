using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;

namespace VFoody.Application.UseCases.Roles.Commands.CreateRole;

public class CreateRoleCommand : ICommand<Unit>
{
    public string Name { get; set; }

}
