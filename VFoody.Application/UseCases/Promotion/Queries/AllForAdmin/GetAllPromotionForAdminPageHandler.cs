using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.AllForAdmin;

public class GetAllPromotionForAdminPageHandler : IQueryHandler<GetAllPromotionForAdminPageQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetAllPromotionForAdminPageQuery> _logger;

    public GetAllPromotionForAdminPageHandler(IDapperService dapperService, ILogger<GetAllPromotionForAdminPageQuery> logger)
    {
        _dapperService = dapperService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetAllPromotionForAdminPageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var parameter = new
            {
                DateFrom = request.DateFrom == default ? null : request.DateFrom.ToString("yyyy-MM-dd"),
                DateTo = request.DateTo.ToString("yyyy-MM-dd"),
                Status = (int)request.Status,
                ApplyType = (int)request.ApplyType,
                Title = request.Title,
                Description = request.Description,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
            
            var listPromotion = await this._dapperService.SelectAsync<AllPromotionResponse>(
                QueryName.SelectAllPromotionForAdminPageWithPaging, parameter)
                .ConfigureAwait(false);
            
            var result = new PaginationResponse<AllPromotionResponse>(listPromotion.ToList(), request.PageIndex, request.PageSize,
                listPromotion.Count() > 0 ? listPromotion.First().TotalItems : 0);
            return Result.Success(result);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal error"));
        }
    }
}