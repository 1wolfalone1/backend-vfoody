using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductImageOfShopOwner;

public class CreateProductImageHandler : ICommandHandler<CreateProductImageCommand, Result>
{
    private readonly IStorageService _storageService;

    public CreateProductImageHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<Result<Result>> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
    {
        return Result.Create( await _storageService.UploadFileAsync(request.File));
    }
}