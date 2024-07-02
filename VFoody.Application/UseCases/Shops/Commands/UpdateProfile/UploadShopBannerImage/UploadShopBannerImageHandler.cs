using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.UpdateProfile.UploadShopBannerImage;

public class UploadShopBannerImageHandler : ICommandHandler<UploadShopBannerImageCommand, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IStorageService _storageService;
    private readonly IMapper _mapper;
    
    
    public UploadShopBannerImageHandler(IShopRepository shopRepository, IUnitOfWork unitOfWork, IStorageService storageService, ICurrentPrincipalService currentPrincipalService, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _currentPrincipalService = currentPrincipalService;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(UploadShopBannerImageCommand request, CancellationToken cancellationToken)
    {
        var shop = await this._shopRepository.GetShopProfileByAccountIdAsync(this._currentPrincipalService.CurrentPrincipalId.Value)
            .ConfigureAwait(false);
        if (shop == default)
            throw new InvalidBusinessException("Không tìm thấy cửa hàng để cập nhật thông tin");
        
        // Upload image to aws
        // Delete old image
        if (shop.BannerUrl != null && shop.BannerUrl.Trim().Length > 0)
        {
            var fileNameImage = shop.BannerUrl.Split("/");
            var isDelete = this._storageService.DeleteFileAsync(fileNameImage[fileNameImage.Length-1]);
        }

        var newBannerUrl = await this._storageService.UploadFileAsync(request.BannerImage);
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            shop.BannerUrl = newBannerUrl;
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            var shopResponse = this._mapper.Map<ShopProfileResponse>(shop);
            return Result.Success(shopResponse);
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            Console.WriteLine(e);
            throw;
        }
    }
}