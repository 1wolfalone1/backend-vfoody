using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, Result>
{
    private readonly ILogger<UpdateProductHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IOptionRepository _optionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductHandler(
        ILogger<UpdateProductHandler> logger,
        ICategoryRepository categoryRepository, IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork,
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository,
        IQuestionRepository questionRepository, IOptionRepository optionRepository)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
        _questionRepository = questionRepository;
        _optionRepository = optionRepository;
    }

    public async Task<Result<Result>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //Begin transaction
            await _unitOfWork.BeginTransactionAsync();

            var accountId = _currentPrincipalService.CurrentPrincipalId;
            var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
            var product = _productRepository.GetIncludeProductCategoryAndQuestionByIdAndShopId(request.Id, shop.Id);
            var newQuestionIds = new List<int>();

            //Return failure when product not found
            if (product == null)
            {
                return Result.Failure(new Error("400", "Product not found."));
            }

            //Update product information
            product.Name = request.Name;
            product.Description = request.Description;
            product.ImageUrl = request.ImgUrl;
            product.Price = request.Price;
            //Update product
            _productRepository.Update(product);

            //Update product category if the new name is not null, not empty
            var categoryIds = product.ProductCategories.Select(category => category.CategoryId).ToList();
            if (request.CategoryIds.Count > 0)
            {
                //Categories in the request that are not in categoryIds for new product category
                var notInCategoryIds = request.CategoryIds.Except(categoryIds).ToList();
                if (notInCategoryIds.Count > 0)
                {
                    //Check existed category
                    var existCategory = await _categoryRepository.CheckExistCategoryByIds(notInCategoryIds);
                    if (!existCategory)
                    {
                        _unitOfWork.RollbackTransaction();
                        return Result.Failure(new Error("400", "Category not found."));
                    }

                    //Save product category
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

                //Categories in categoryIds that are not in the request for remove product category
                var categoryIdsNotInRequest = categoryIds.Except(request.CategoryIds).ToList();
                if (categoryIdsNotInRequest.Count > 0)
                {
                    var productCategoriesRemove =
                        product.ProductCategories.Where(pc => categoryIdsNotInRequest.Contains(pc.CategoryId)).ToList();
                    _productCategoryRepository.RemoveRange(productCategoriesRemove);
                }
            }
            else
            {
                _unitOfWork.RollbackTransaction();
                return Result.Failure(new Error("400", "Category not found."));
            }

            //Remove all option and question present in product when update questions null or empty
            if ((request.Questions == null || request.Questions.Count == 0) && product.Questions.Count > 0)
            {
                //Remove option
                var questionIds = product.Questions.Select(question => question.Id).ToList();
                var optionsRemove = await _optionRepository.GetByQuestionIds(questionIds);
                optionsRemove.ForEach(option => option.Status = (int)OptionStatus.Delete);
                foreach (var productQuestion in product.Questions)
                {
                    productQuestion.Status = (int)QuestionStatus.Delete;
                }

                // Update status question and option to delete
                _optionRepository.UpdateRange(optionsRemove);
                _questionRepository.UpdateRange(product.Questions);
            }

            if (request.Questions != null && request.Questions.Count > 0)
            {
                // Get questions that have a present ID for updating
                var questionsRequestUpdate = request.Questions
                    .Where(q => q.Id.HasValue && q.Id.Value > 0)
                    .ToList();

                var questionsRequestUpdateId = new List<int>();
                if (questionsRequestUpdate.Count > 0)
                {
                    questionsRequestUpdateId = questionsRequestUpdate.Select(q => q.Id!.Value).ToList();
                }

                var questionsRequestAddNew = request.Questions
                    .Where(q => !q.Id.HasValue || q.Id.Value == 0)
                    .ToList();

                if (questionsRequestAddNew.Count > 0)
                {
                    foreach (var requestQuestion in questionsRequestAddNew)
                    {
                        var question = new Question
                        {
                            ProductId = product.Id,
                            Description = requestQuestion.Description,
                            Status = (int)QuestionStatus.Active,
                            QuestionType = requestQuestion.Type
                        };
                        _questionRepository.AddAsync(question);
                        await _unitOfWork.SaveChangesAsync();
                        newQuestionIds.Add(question.Id);
                        foreach (var requestOption in requestQuestion.Options)
                        {
                            var option = new Option
                            {
                                QuestionId = question.Id,
                                Description = requestOption.Description,
                                IsPricing = requestOption.IsPricing,
                                Price = requestOption.Price,
                                ImageUrl = requestOption.ImgUrl,
                                Status = (int)OptionStatus.Active
                            };
                            _optionRepository.AddAsync(option);
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                }

                if (questionsRequestUpdateId.Count == 0 && product.Questions.Count > 0)
                {
                    product.Questions.ToList().ForEach(question =>
                    {
                        question.Status = (int)OptionStatus.Delete;
                        _questionRepository.Update(question);
                    });
                }

                if (questionsRequestUpdateId.Count > 0)
                {
                    //Check existed question
                    var existedQuestion = await _questionRepository.CheckExistedQuestionByIdsAndProductId(
                        questionsRequestUpdateId, product.Id
                    );
                    //Return failure when question not existed
                    if (!existedQuestion)
                    {
                        _unitOfWork.RollbackTransaction();
                        return Result.Failure(new Error("400", "Question not found."));
                    }

                    var questionsUpdate = product.Questions.ToList()
                        .Where(question => questionsRequestUpdateId.Contains(question.Id))
                        .ToList();

                    if (questionsUpdate.Count > 0)
                    {
                        foreach (var questionRequest in questionsRequestUpdate)
                        {
                            var question =
                                await _questionRepository.GetQuestionIncludeOptionById(questionRequest.Id!.Value);
                            question!.Description = questionRequest.Description;
                            question.QuestionType = questionRequest.Type;
                            question.Status = questionRequest.Status;
                            // Update question
                            _questionRepository.Update(question);

                            if (questionRequest.Options.Count == 0)
                            {
                                _unitOfWork.RollbackTransaction();
                                return Result.Failure(new Error("400", "Option is required."));
                            }

                            var optionsRequestUpdate = questionRequest.Options
                                .Where(o => o.Id.HasValue && o.Id.Value > 0)
                                .ToList();
                            var optionsRequestUpdateId = optionsRequestUpdate.Select(o => o.Id!.Value).ToList();
                            var options = await _optionRepository.GetByQuestionIds([question.Id]);
                            var optionsRequestAddNew = questionRequest.Options
                                .Where(o => !o.Id.HasValue || o.Id.Value == 0)
                                .ToList();
                            if (optionsRequestAddNew.Count > 0)
                            {
                                optionsRequestAddNew.ForEach(requestOption =>
                                {
                                    var option = new Option
                                    {
                                        Description = requestOption.Description,
                                        IsPricing = requestOption.IsPricing,
                                        Price = requestOption.Price,
                                        ImageUrl = requestOption.ImgUrl,
                                        Status = (int)OptionStatus.Active
                                    };
                                    _optionRepository.AddAsync(option);
                                });
                            }

                            if (optionsRequestUpdateId.Count == 0 && options.Count > 0)
                            {
                                options.ForEach(option =>
                                {
                                    option.Status = (int)OptionStatus.Delete;
                                    _optionRepository.Update(option);
                                });
                            }

                            if (optionsRequestUpdateId.Count > 0)
                            {
                                var existedOption = await _optionRepository.CheckExistedOptionByIdsAndQuestionId(
                                    optionsRequestUpdateId, question.Id
                                );
                                //Return failure when option not existed
                                if (!existedOption)
                                {
                                    _unitOfWork.RollbackTransaction();
                                    return Result.Failure(new Error("400", "Option not found."));
                                }

                                var optionsUpdate = options
                                    .Where(option => optionsRequestUpdateId.Contains(option.Id))
                                    .ToList();
                                if (optionsUpdate.Count > 0)
                                {
                                    optionsUpdate.ForEach(option =>
                                    {
                                        var optionRequest =
                                            optionsRequestUpdate.Single(optionRequest => optionRequest.Id == option.Id);
                                        option.Description = optionRequest.Description;
                                        option.IsPricing = optionRequest.IsPricing;
                                        option.ImageUrl = optionRequest.ImgUrl;
                                        option.Price = optionRequest.Price;
                                        option.Status = optionRequest.Status;
                                        //Update option
                                        _optionRepository.Update(option);
                                    });
                                }

                                var optionsRemove = options
                                    .Where(option => !optionsRequestUpdateId.Contains(option.Id))
                                    .ToList();
                                if (optionsRemove.Count > 0)
                                {
                                    optionsRemove.ForEach(option =>
                                    {
                                        option.Status = (int)OptionStatus.Delete;
                                        _optionRepository.Update(option);
                                    });
                                }
                            }
                        }
                    }

                    var questionsRemove = product.Questions.ToList()
                        .Where(question => !questionsRequestUpdateId.Contains(question.Id) && !newQuestionIds.Contains(question.Id))
                        .ToList();

                    if (questionsRemove.Count > 0)
                    {
                        foreach (var question in questionsRemove)
                        {
                            var questionRemove = await _questionRepository.GetQuestionIncludeOptionById(question.Id);
                            questionRemove!.Status = (int)QuestionStatus.Delete;
                            _questionRepository.Update(questionRemove);
                            foreach (var option in questionRemove.Options)
                            {
                                option.Status = (int)OptionStatus.Delete;
                                _optionRepository.Update(option);
                            }
                        }
                    }
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