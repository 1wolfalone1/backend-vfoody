using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Roles.Commands.UpdateRole;

public class UpdateRoleHandler : ICommandHandler<UpdateRoleCommand, Result>
{
    private readonly IRoleRepository roleRepository;
    private readonly IUnitOfWork unitOfWork;

    public UpdateRoleHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        this.roleRepository = roleRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        await this.unitOfWork.BeginTransactionAsync();
        try
        {
            Role role = await this.roleRepository.GetByIdAsync(request.Role.Id);
            role.Name = request.Role.Name;
            this.roleRepository.Update(role);
            await this.unitOfWork.CommitTransactionAsync();
            return Result.Success(Unit.Value);
        }catch (Exception ex)
        {
            this.unitOfWork.RollbackTransaction();
            return Result.Failure(new Error("500", "Fail"));
        }
    }
}
