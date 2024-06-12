using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, Result>
{
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly IStorageService _storageService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IShopRepository _shopRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IUnitOfWork _unitOfWork;


    public CreateProductHandler(ILogger<CreateProductHandler> logger, IStorageService storageService,
        ICategoryRepository categoryRepository, IProductRepository productRepository,
        IShopRepository shopRepository, ICurrentPrincipalService currentPrincipalService,
        IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _storageService = storageService;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _shopRepository = shopRepository;
        _currentPrincipalService = currentPrincipalService;
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //1. Check existed category
        var existCategory = await _categoryRepository.CheckExistCategoryByIds(request.CategoryIds);
        if (!existCategory)
        {
            return Result.Failure(new Error("400", "Category not found."));
        }

        //2. Upload product image
        var imgUrl = await _storageService.UploadFileAsync(request.File);

        //3. Create new product
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        var product = new Domain.Entities.Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = imgUrl,
            TotalOrder = 0,
            Status = (int)ProductStatus.Active,
            ShopId = shop.Id
        };
        try
        {
            //Begin transaction
            await _unitOfWork.BeginTransactionAsync();
            //3.1 Save product
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            //4. Create product category
            request.CategoryIds.ForEach(id =>
            {
                ProductCategory productCategory = new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = id
                };
                //4.1 Save product category
                _productCategoryRepository.AddAsync(productCategory);
            });

            //5. Update total product of shop
            shop.TotalProduct += 1;
            //5.1 Update shop information
            _shopRepository.Update(shop);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Create("Create new product successfully!");
        }
        catch (Exception e)
        {
            //Rollback when exception
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }
}