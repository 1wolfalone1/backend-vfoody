using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Category.Queries;

public class GetAllCategoryHandler : IQueryHandler<GetAllCategoryQuery, Result>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<GetAllCategoryHandler> _logger;

    public GetAllCategoryHandler(ICategoryRepository categoryRepository, ILogger<GetAllCategoryHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return Result.Success(await this._categoryRepository.GetAsync().ConfigureAwait(false));
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("404", "Not found any category"));
        }
    }
}