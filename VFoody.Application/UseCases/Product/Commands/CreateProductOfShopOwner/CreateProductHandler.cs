using System.Collections;
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
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IShopRepository _shopRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IUnitOfWork _unitOfWork;


    public CreateProductHandler(ILogger<CreateProductHandler> logger,
        ICategoryRepository categoryRepository, IProductRepository productRepository,
        IShopRepository shopRepository, ICurrentPrincipalService currentPrincipalService,
        IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork,
        IQuestionRepository questionRepository)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _shopRepository = shopRepository;
        _currentPrincipalService = currentPrincipalService;
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
        _questionRepository = questionRepository;
    }

    public async Task<Result<Result>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //1. Check existed category
        var existCategory = await _categoryRepository.CheckExistCategoryByIds(request.CategoryIds);
        if (!existCategory)
        {
            return Result.Failure(new Error("400", "Category not found."));
        }

        //2. Create new product
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        var product = new Domain.Entities.Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImgUrl,
            TotalOrder = 0,
            Status = (int)ProductStatus.Active,
            ShopId = shop.Id
        };

        //5. Create question if present
        var questions = new List<Question>();
        if (request.Questions != null && request.Questions.Count > 0)
        {
            request.Questions.ForEach(requestQuestion =>
            {
                var options = new List<Option>();
                requestQuestion.Options.ForEach(requestOption =>
                {
                    var option = new Option
                    {
                        Description = requestOption.Description,
                        IsPricing = requestOption.IsPricing,
                        Price = requestOption.Price,
                        ImageUrl = requestOption.ImgUrl,
                        Status = (int)OptionStatus.Active
                    };
                    options.Add(option);
                });
                var question = new Question
                {
                    Description = requestQuestion.Description,
                    Status = (int)QuestionStatus.Active,
                    QuestionType = requestQuestion.Type,
                    Options = options
                };
                questions.Add(question);
            });
        }

        try
        {
            //Begin transaction
            await _unitOfWork.BeginTransactionAsync();
            //2.1 Save product
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            //Save product question if present
            if (questions.Count > 0)
            {
                questions.ForEach(q => q.ProductId = product.Id);
                await _questionRepository.AddRangeAsync(questions);
            }
            //3. Create product category
            request.CategoryIds.ForEach(id =>
            {
                var productCategory = new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = id
                };
                //3.1 Save product category
                _productCategoryRepository.AddAsync(productCategory);
            });

            shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
            //4. Update total product of shop
            shop.TotalProduct += 1;
            //4.1 Update shop information
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