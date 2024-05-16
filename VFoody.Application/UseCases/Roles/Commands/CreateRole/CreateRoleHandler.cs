using FluentValidation;
using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Roles.Commands.CreateRole;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Roles.Commands.CreateRole;

public class CreateRoleHandler : ICommandHandler<CreateRoleCommand, Unit>
{
    private readonly IRoleRepository roleRepository;
    private readonly IUnitOfWork unitOfWork;


    public CreateRoleHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        this.roleRepository = roleRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        Role role = new Role();
        role.Name = request.Name;
        await this.unitOfWork.BeginTransactionAsync();
        await this.roleRepository.AddAsync(role);
        await this.unitOfWork.CommitTransactionAsync();

        return Result.Success(Unit.Value);
        
    } 
}

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull();
    }
}