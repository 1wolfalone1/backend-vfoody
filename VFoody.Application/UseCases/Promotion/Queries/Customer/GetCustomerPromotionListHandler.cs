using ArtHubBO.Payload;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.Customer;

public class GetCustomerPromotionListHandler : IQueryHandler<GetCustomerPromotionListQuery, Result>
{
    private readonly IDapperService dapperService;
    private readonly ILogger<GetCustomerPromotionListHandler> _logger;

    public GetCustomerPromotionListHandler(IDapperService dapperService, ILogger<GetCustomerPromotionListHandler> logger)
    {
        this.dapperService = dapperService;
        _logger = logger;
    }


    public async Task<Result<Result>> Handle(GetCustomerPromotionListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await dapperService.SelectAsync<SelectUserPromotionDTO>(QueryName.SelectUserPromotions, new
            {
                request.AccountId,
                request.Status,
                request.StartDate,
                request.EndDate,
                request.Available,
                request.PageIndex,
                request.PageSize,                
            }).ConfigureAwait(false);

            var result = new PaginationResponse<SelectUserPromotionDTO>(list.ToList(), request.PageIndex, request.PageSize, list.First().TotalItems);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}
