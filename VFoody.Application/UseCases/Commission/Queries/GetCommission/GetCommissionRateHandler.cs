using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Commission.Queries.GetCommission;

public class GetCommissionRateHandler : IQueryHandler<GetCommissionRateQuery, Result>
{
    private readonly ICommissionConfigRepository _commissionConfigRepository;

    public GetCommissionRateHandler(ICommissionConfigRepository commissionConfigRepository)
    {
        _commissionConfigRepository = commissionConfigRepository;
    }

    public async Task<Result<Result>> Handle(GetCommissionRateQuery request, CancellationToken cancellationToken)
    {
        var commissionConfig = _commissionConfigRepository.Get()
            .OrderByDescending(c => c.CreatedDate)
            .First();
        return Result.Success(commissionConfig);
    }
}