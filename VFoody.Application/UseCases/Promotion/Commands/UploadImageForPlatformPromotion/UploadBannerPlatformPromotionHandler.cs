using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.UploadImageForPlatformPromotion;

public class UploadBannerPlatformPromotionHandler : ICommandHandler<UploadBannerPlatformPromotionCommand,Result>
{
    private readonly IStorageService _storageService;
    private readonly IPlatformPromotionRepository _platformPromotionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UploadBannerPlatformPromotionHandler> _logger;
    private readonly IMapper _mapper;

    public UploadBannerPlatformPromotionHandler(IStorageService storageService, IPlatformPromotionRepository platformPromotionRepository, IUnitOfWork unitOfWork, ILogger<UploadBannerPlatformPromotionHandler> logger, IMapper mapper)
    {
        _storageService = storageService;
        _platformPromotionRepository = platformPromotionRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(UploadBannerPlatformPromotionCommand request, CancellationToken cancellationToken)
    {
        var imagePath = await this._storageService.UploadFileAsync(request.BannerImage);
        return Result.Success(new
        {
            ImageUrl = imagePath
        });
    }
}