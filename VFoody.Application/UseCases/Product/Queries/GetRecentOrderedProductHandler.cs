using ArtHubBO.Payload;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Application.UseCases.Shop.Queries;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries;

public class GetRecentOrderedProductHandler : IQueryHandler<GetRecentOrderedProductQuery, Result>
{
    private readonly IDapperService dapperService;
    private readonly IAccountService accountService;
    private readonly ITestService testService;

    public GetRecentOrderedProductHandler(IDapperService dapperService, ITestService testService)
    {
        this.dapperService = dapperService;
        this.testService = testService;
    }


    public async Task<Result<Result>> Handle(GetRecentOrderedProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await this.dapperService.SelectAsync<SelectSimpleProductDTO>(QueryName.SelectRecentOrderedProduct, new
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                UserPhoneNumber = request.Phone
            }).ConfigureAwait(false);

            var result = new PaginationResponse<SelectSimpleProductDTO>(list.ToList(), request.PageIndex, request.PageSize, list.First().TotalPages);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            // logging exception
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}
