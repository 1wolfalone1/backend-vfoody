using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Commission.Commands.CreateCommission;

public class CreateCommissionHandler : ICommandHandler<CreateCommissionCommand, Result>
{
    private readonly ICommissionConfigRepository _commissionConfigRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCommissionHandler(ICommissionConfigRepository commissionConfigRepository, IUnitOfWork unitOfWork)
    {
        _commissionConfigRepository = commissionConfigRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(CreateCommissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            CommissionConfig commissionConfig = new CommissionConfig
            {
                CommissionRate = request.CommissionRate
            };
            await _commissionConfigRepository.AddAsync(commissionConfig);
            await _unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Success("Update commission rate successfully.");
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            throw new Exception(e.Message);
        }
    }
}