using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Queries.LoginToShop;

public class LoginToShopHandler : IQueryHandler<LoginToShopQuery, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginToShopHandler(IShopRepository shopRepository, ICurrentPrincipalService currentPrincipalService,
        IJwtTokenService jwtTokenService, IAccountRepository accountRepository)
    {
        _shopRepository = shopRepository;
        _currentPrincipalService = currentPrincipalService;
        _jwtTokenService = jwtTokenService;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Result>> Handle(LoginToShopQuery request, CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var account = _accountRepository.GetById(accountId!);
        var shop = _shopRepository.Get().Include(s => s.Building)
            .FirstOrDefault(s => s.AccountId == accountId!.Value && s.Status != (int)ShopStatus.Delete);
        if (shop == null)
        {
            return Result.Failure(new Error("1", "Tài khoản không tồn tại shop."));
        }

        if (shop.Status == (int)ShopStatus.UnActive)
        {
            return Result.Failure(new Error("2", "Tài khoản shop chưa được chấp thuận."));
        }

        if (shop.Status == (int)ShopStatus.Ban)
        {
            return Result.Failure(new Error("3", "Tài khoản shop đã bị ban."));
        }

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
                Id = shop.Building.Id,
                Address = shop.Building.Name,
                Latitude = shop.Building.Latitude!.Value,
                Longitude = shop.Building.Longitude!.Value
            }
        };
        return Result.Success(shopOwnerResponse);
    }
}