using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.UpdateProfile.UploadShopLogoImage;

public class UploadShopLogoImageHandler : ICommandHandler<UploadShopLogoImageCommand, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IStorageService _storageService;
    private readonly IMapper _mapper;
    
    
    public UploadShopLogoImageHandler(IShopRepository shopRepository, IUnitOfWork unitOfWork, IStorageService storageService, ICurrentPrincipalService currentPrincipalService, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _currentPrincipalService = currentPrincipalService;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(UploadShopLogoImageCommand request, CancellationToken cancellationToken)
    {
        var shop = await this._shopRepository.GetShopProfileByAccountIdAsync(this._currentPrincipalService.CurrentPrincipalId.Value)
            .ConfigureAwait(false);
        if (shop == default)
            throw new InvalidBusinessException("Không tìm thấy cửa hàng để cập nhật thông tin");
        
        // Upload image to aws
        // Delete old image
        if (shop.LogoUrl != null && shop.LogoUrl.Trim().Length > 0)
        {
            var fileNameImage = shop.LogoUrl.Split("/");
            var isDelete = this._storageService.DeleteFileAsync(fileNameImage[fileNameImage.Length-1]);
        }

        var newLogoUrl = await this._storageService.UploadFileAsync(request.LogoImage);
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            shop.LogoUrl = newLogoUrl;
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