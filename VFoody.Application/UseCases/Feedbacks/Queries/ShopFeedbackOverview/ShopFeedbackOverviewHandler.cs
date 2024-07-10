using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Feedbacks.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Feedbacks.Queries.ShopFeedbackOverview;

public class ShopFeedbackOverviewHandler : IQueryHandler<ShopFeedbackOverviewQuery, Result>
{
    private readonly IDapperService _dapperService;

    public ShopFeedbackOverviewHandler(IDapperService dapperService)
    {
        _dapperService = dapperService;
    }

    public async Task<Result<Result>> Handle(ShopFeedbackOverviewQuery request, CancellationToken cancellationToken)
    {
        var listShopFeedbackOverview = await this._dapperService.SingleOrDefaultAsync<ShopFeedbackOverviewResponse>(
            QueryName.GetShopOverviewFeedback,
            new
            {
                ShopId = request.ShopId
            });
        return Result.Success(listShopFeedbackOverview);
    }
}