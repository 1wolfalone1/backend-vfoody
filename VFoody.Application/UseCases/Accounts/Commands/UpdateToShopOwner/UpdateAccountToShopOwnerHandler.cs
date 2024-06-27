using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateToShopOwner;

public class UpdateAccountToShopOwnerHandler : ICommandHandler<UpdateAccountToShopOwnerCommand, Result>
{
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IBuildingRepository _buildingRepository;
    private readonly IStorageService _storageService;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public UpdateAccountToShopOwnerHandler(
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository,
        IUnitOfWork unitOfWork, IStorageService storageService,
        IBuildingRepository buildingRepository, IJwtTokenService jwtTokenService, IAccountRepository accountRepository)
    {
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _buildingRepository = buildingRepository;
        _jwtTokenService = jwtTokenService;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Result>> Handle(UpdateAccountToShopOwnerCommand request,
        CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var account = _accountRepository.GetById(accountId!);

        //Check existed shop with this account
        var existedShop = _shopRepository.Get().Any(s => s.AccountId == accountId!.Value);
        if (existedShop)
        {
            return Result.Failure(new Error("400", "Tài khoản đã tồn tại shop."));
        }

        //Check existed shop phone
        var existedShopPhone = _shopRepository.Get().Any(s => s.PhoneNumber == request.PhoneNumber);
        if (existedShopPhone)
        {
            return Result.Failure(new Error("400", "Số điện thoại shop đã tồn tại."));
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            Building building = new Building
            {
                Name = request.BuildingName,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            await _buildingRepository.AddAsync(building);
            await _unitOfWork.SaveChangesAsync();
            var bannerUrl = await _storageService.UploadFileAsync(request.BannerFile);
            var logoUrl = await _storageService.UploadFileAsync(request.LogoFile);
            Domain.Entities.Shop shop = new Domain.Entities.Shop
            {
                Name = request.ShopName,
                BannerUrl = bannerUrl,
                LogoUrl = logoUrl,
                Description = request.Description,
                Balance = 0,
                PhoneNumber = request.PhoneNumber,
                ActiveFrom = request.ActiveFrom,
                ActiveTo = request.ActiveTo,
                Active = 0,
                TotalOrder = 0,
                TotalProduct = 0,
                TotalRating = 0,
                TotalStar = 0,
                Status = (int)ShopStatus.UnActive,
                MinimumValueOrderFreeship = request.MinimumValueOrderFreeship,
                ShippingFee = request.ShippingFee,
                AccountId = accountId!.Value,
                BuildingId = building.Id
            };
            await _shopRepository.AddAsync(shop);
            await _unitOfWork.SaveChangesAsync();
            account.RoleId = (int)Domain.Enums.Roles.Shop;
            _accountRepository.Update(account);
            await _unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            UpdateAccountToShopOwnerResponse shopOwnerResponse = new UpdateAccountToShopOwnerResponse
            {
                Id = shop.Id,
                ShopName = shop.Name,
                BannerUrl = shop.BannerUrl!,
                LogoUrl = shop.LogoUrl!,
                Description = shop.Description!,
                PhoneNumber = shop.PhoneNumber,
                ActiveFrom = shop.ActiveFrom,
                ActiveTo = shop.ActiveTo,
                MinimumValueOrderFreeship = shop.MinimumValueOrderFreeship,
                ShippingFee = shop.ShippingFee,
                AccessTokenResponse = new AccessTokenResponse
                {
                    AccessToken = _jwtTokenService.GenerateJwtToken(account!),
                    RefreshToken = _jwtTokenService.GenerateJwtRefreshToken(account!)
                },
                Building = new BuildingResponse
                {
                    Id = building.Id,
                    Address = building.Name,
                    Latitude = building.Latitude!.Value,
                    Longitude = building.Longitude!.Value
                }
            };
            return Result.Success(shopOwnerResponse);
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            throw new Exception(e.Message);
        }
    }
}