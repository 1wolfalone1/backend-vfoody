using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Feedbacks.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Feedbacks.Queries.ShopFeedbacks;

public class GetAllShopFeedbackHandler : IQueryHandler<GetAllShopFeedbackQuery,Result>
{
    private readonly IDapperService _dapperService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly ILogger<GetAllShopFeedbackHandler> _logger;

    public GetAllShopFeedbackHandler(IDapperService dapperService, IFeedbackRepository feedbackRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork, ILogger<GetAllShopFeedbackHandler> logger)
    {
        _dapperService = dapperService;
        _feedbackRepository = feedbackRepository;
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetAllShopFeedbackQuery request, CancellationToken cancellationToken)
    {

        try
        {
            var listFeedBack = await this._dapperService
                .SelectAsync<PaginationResponse<ShopFeedbackDTO>, ShopFeedbackDTO, PaginationResponse<ShopFeedbackDTO>>(
                    QueryName.SelectShopFeedbacks, (parent, child) =>
                    {
                        parent.Items ??= new List<ShopFeedbackDTO>();
                        parent.Items.Add(child);
                        return parent;
                    }, new
                    {
                        ShopId = request.Id,
                        PageIndex = request.PageIndex,
                        PageSize = request.PageSize
                    }, "AccountId").ConfigureAwait(false);
            return Result.Success(listFeedBack);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("404", "Not found any feedback"));
        }
    }
}