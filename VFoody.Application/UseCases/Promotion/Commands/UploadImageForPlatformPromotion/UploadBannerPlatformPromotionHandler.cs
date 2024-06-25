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
        var platformPromotion = this._platformPromotionRepository.GetById(request.Id);
        // Delete old image
        var fileNameImage = platformPromotion.BannerUrl.Split("/");
        if (fileNameImage.Length > 0)
        {
            var isDelete = this._storageService.DeleteFileAsync(fileNameImage[fileNameImage.Length-1]);
        }

        var imagePath = await this._storageService.UploadFileAsync(request.BannerImage);
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            platformPromotion.BannerUrl = imagePath;
            this._platformPromotionRepository.Update(platformPromotion);
            var response = this._mapper.Map<AllPromotionResponse>(platformPromotion);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Success(response);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            this._unitOfWork.RollbackTransaction();
            throw new Exception(e.Message);
        }
    }
}