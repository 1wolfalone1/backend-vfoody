using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ManageShop;

public class GetAllShopHandler : IQueryHandler<GetAllShopQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetAllShopHandler> _logger;
    private readonly IMapper _mapper;

    public GetAllShopHandler(IMapper mapper, IDapperService dapperService, ILogger<GetAllShopHandler> logger)
    {
        _mapper = mapper;
        _dapperService = dapperService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetAllShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var parameter = new
            {
                DeleteStatus = (int) ShopStatus.Delete,
                OrderSuccessful = (int) OrderStatus.Successful,
                SearchValue = request.SearchValue,
                FilterByTime = request.FilterByTime,
                DateFrom = request.DateFrom?.ToString("yyyy-MM-dd"),
                DateTo = request.DateTo?.ToString("yyyy-MM-dd"),
                OrderBy = request.OrderBy,
                Direction = request.Direction,
                Offset = (request.PageIndex - 1) * request.PageSize,
                PageSize = request.PageSize
            };


            var orders = await _dapperService.SelectAsync<ManageShopDto>(
                    QueryName.SelectAllShopByCondition, parameter)
                .ConfigureAwait(false);

            var result = new PaginationResponse<ManageShopResponse>(_mapper.Map<List<ManageShopResponse>>(
                orders.ToList()), request.PageIndex, request.PageSize, orders.First().TotalCount);
            return Result.Success(result);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal error"));
        }
    }
}