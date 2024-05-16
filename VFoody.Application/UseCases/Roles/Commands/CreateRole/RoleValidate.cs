using FluentValidation;
using VFoody.Domain.Entities;

namespace VFoody.Application.UseCases.Roles.Commands.CreateRole
{
    public class RoleValidate : AbstractValidator<Role>
    {
        public RoleValidate()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
