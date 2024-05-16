using FluentValidation;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.UseCases.Roles.Commands.CreateRole;
using VFoody.Application.UseCases.Roles.Commands.UpdateRole;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : ICommand<Result>
    {
        public UpdateRole Role { get; set; }
    }
}

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Role.Id).NotNull();
        RuleFor(x => x.Role.Name).NotEmpty();
    }
}
