using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, Result>
{
    private readonly ILogger<UpdateProductHandler> _logger;
    private readonly IStorageService _storageService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductHandler(
        ILogger<UpdateProductHandler> logger, IStorageService storageService,
        ICategoryRepository categoryRepository, IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork,
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository)
    {
        _logger = logger;
        _storageService = storageService;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        var product = _productRepository.GetProductIncludeProductCategoryByIdAndShopId(request.Id, shop.Id);

        //1. Return failure when product not found
        if (product == null)
        {
            return Result.Failure(new Error("400", "Product not found."));
        }

        //2. Update product information
        //2.1 Update the product name if the new name is not null, not empty,
        // and is different from the existing name.
        if (!string.IsNullOrEmpty(request.Name) && request.Name != product.Name)
        {
            product.Name = request.Name;
        }

        //2.2 Update the product description if the new description is not null, not empty,
        // and is different from the existing description.
        if (!string.IsNullOrEmpty(request.Description) && request.Description != product.Description)
        {
            product.Description = request.Description;
        }

        //2.3 Update new image if file not null
        if (request.File != null)
        {
            //2.3.1 Upload product image
            var imgUrl = await _storageService.UploadFileAsync(request.File);
            product.ImageUrl = imgUrl;
        }

        //2.4 Update price if the request has a price value, the value is greater than 0 and is different from the existing price.
        if (request.Price > 0 && Math.Abs(request.Price - product.Price) > 0.00001)
        {
            product.Price = request.Price;
        }

        try
        {
            //Begin transaction
            await _unitOfWork.BeginTransactionAsync();
            //2.5 Update product
            _productRepository.Update(product);

            //2.6 Update the product category if the new name is not null, not empty
            var categoryIds = product.ProductCategories.Select(category => category.CategoryId).ToList();
            if (request.CategoryIds != null && request.CategoryIds.Count > 0)
            {
                //2.6.1 Categories in the request that are in categoryIds
                var inCategoryIds = request.CategoryIds.Intersect(categoryIds).ToList();

                //2.6.1 Categories in the request that are not in categoryIds for new product category
                var notInCategoryIds = request.CategoryIds.Except(categoryIds).ToList();
                if (notInCategoryIds.Count > 0)
                {
                    //2.6.1.1 Check existed category
                    var existCategory = await _categoryRepository.CheckExistCategoryByIds(notInCategoryIds);
                    if (!existCategory)
                    {
                        _unitOfWork.RollbackTransaction();
                        return Result.Failure(new Error("400", "Category not found."));
                    }

                    //2.6.1.2 Save product category
                    notInCategoryIds.ForEach(id =>
                    {
                        ProductCategory productCategory = new ProductCategory
                        {
                            ProductId = product.Id,
                            CategoryId = id
                        };
                        _productCategoryRepository.AddAsync(productCategory);
                    });
                }

                //2.6.2 Categories in categoryIds that are not in the request for remove product category
                var categoryIdsNotInRequest = categoryIds.Except(request.CategoryIds).ToList();
                if (categoryIdsNotInRequest.Count > 0)
                {
                    var productCategoriesRemove =
                        product.ProductCategories.Where(pc => categoryIdsNotInRequest.Contains(pc.CategoryId)).ToList();
                    _productCategoryRepository.RemoveRange(productCategoriesRemove);
                }
            }

            //Commit transaction
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            //Rollback when exception
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }

        return Result.Success("Update product successfully!");
    }
}