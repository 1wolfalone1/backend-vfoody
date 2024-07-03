using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductStatusOfShopOwner;

public class UpdateProductStatusHandler : ICommandHandler<UpdateProductStatusCommand, Result>
{
    private readonly ILogger<UpdateProductStatusHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductStatusHandler(
        ILogger<UpdateProductStatusHandler> logger, IProductRepository productRepository,
        IUnitOfWork unitOfWork, IOrderRepository orderRepository,
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        var product = await _productRepository.GetProductByIdAndShopId(request.Id, shop.Id);

        //1. Return failure when product not found
        if (product == null)
        {
            return Result.Failure(new Error("400", "Không tìm thấy sản phẩm."));
        }

        // 2. Check if the product is currently being ordered
        var isProductInOrder = await _orderRepository.CheckInOrderByProductId(request.Id);
        if (isProductInOrder && request.Status == ProductStatus.Delete)
        {
            // If the product is in the process of being ordered, prevent deletion and return an error
            return Result.Failure(new Error("400", "Sản phẩm đang trong đơn hàng, không thể xóa."));
        }

        //3. Update product status to delete
        try
        {
            //Begin transaction
            await _unitOfWork.BeginTransactionAsync();
            product.Status = (int) request.Status;
            _productRepository.Update(product);
            //Commit transaction
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            //Rollback when exception
            _unitOfWork.RollbackTransaction();
            _logger.LogError(ex, ex.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }

        return Result.Success("Cập nhật trạng thái thành công");
    }
}