using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Commands.CustomerCancels;

public class CustomerCancelHandler : ICommandHandler<CustomerCancelCommand,Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CustomerCancelHandler> _logger;

    public CustomerCancelHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, ILogger<CustomerCancelHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(CustomerCancelCommand request, CancellationToken cancellationToken)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var order = this._orderRepository.Get(predicate: a => a.Id == request.Id
                                                                  && a.Status == (int)OrderStatus.OrderPLaced).FirstOrDefault();
            if (order == null)
            {
                return Result.Failure(new Error("404", $"Không tìm thấy order với id: {request.Id}"));
            }

            order.Status = (int)OrderStatus.Cancelled;

            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Success("Hủy order thành công");
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            this._unitOfWork.RollbackTransaction();
            return Result.Failure(new Error("500", "Hủy order thất bại"));
        }
    }
}