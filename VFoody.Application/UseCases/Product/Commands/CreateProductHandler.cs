using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, Result>
{
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly IStorageService _storageService;

    public CreateProductHandler(ILogger<CreateProductHandler> logger, IStorageService storageService)
    {
        _logger = logger;
        _storageService = storageService;
    }

    public async Task<Result<Result>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        return Result.Create( await _storageService.UploadFileAsync(request.CreateProductRequest.File));
    }
}