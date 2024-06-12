using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.DeleteProductOfShopOwner;

public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, Result>
{
    private readonly ILogger<DeleteProductHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductHandler(
        ILogger<DeleteProductHandler> logger, IProductRepository productRepository,
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

    public async Task<Result<Result>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        var product = await _productRepository.GetProductByIdAndShopId(request.Id, shop.Id);

        //1. Return failure when product not found
        if (product == null)
        {
            return Result.Failure(new Error("400", "Product not found."));
        }

        // 2. Check if the product is currently being ordered
        var isProductInOrder = await _orderRepository.CheckInOrderByProductId(request.Id);
        if (isProductInOrder)
        {
            // If the product is in the process of being ordered, prevent deletion and return an error
            return Result.Failure(new Error("400", "Product is currently being ordered and cannot be deleted."));
        }

        //3. Update product status to delete
        try
        {
            //Begin transaction
            await _unitOfWork.BeginTransactionAsync();
            product.Status = (int)ProductStatus.Delete;
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

        return Result.Success("Delete product successfully!");
    }
}