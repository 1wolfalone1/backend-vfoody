using System.Runtime.CompilerServices;
using MediatR;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Feedbacks.Queries.ShopFeedbacks;

public class GetAllShopFeedbackHandler : IQueryHandler<GetAllShopFeedbackQuery,Result>
{
    private readonly IDapperService _dapperService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;

    public GetAllShopFeedbackHandler(IDapperService dapperService, IFeedbackRepository feedbackRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
    {
        _dapperService = dapperService;
        _feedbackRepository = feedbackRepository;
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(GetAllShopFeedbackQuery request, CancellationToken cancellationToken)
    {
        List<OrderDetail> order = new List<OrderDetail>
        {
            new OrderDetail
            {
                ProductId = 1,
                Quantity = 1,
                Price = 10,
                OrderId = 11,
            },
            new OrderDetail
            {
                ProductId = 2,
                Quantity = 1,
                Price = 10,
                OrderId = 11,
            },
            new OrderDetail
            {
                ProductId = 3,
                Quantity = 1,
                Price = 10,
                OrderId = 11,
            }
        };

        Feedback feed = new Feedback
        {
            OrderId = 11,
            AccountId = 1,
            Rating = 2,
            Comment = "This is soo good"
        };

        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        await this._orderDetailRepository.AddRangeAsync(order);
        await this._feedbackRepository.AddAsync(feed);
        await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
        return Result.Success(Unit.Value);
    }
}