using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Commands.UpdateProfile.UpdateInfo;

public class UpdateInfoShopHandler : ICommandHandler<UpdateInfoShopCommand, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ILogger<UpdateInfoShopHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateInfoShopHandler(IShopRepository shopRepository, IUnitOfWork unitOfWork, ICurrentPrincipalService currentPrincipalService, ILogger<UpdateInfoShopHandler> logger, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _unitOfWork = unitOfWork;
        _currentPrincipalService = currentPrincipalService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(UpdateInfoShopCommand request, CancellationToken cancellationToken)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        var shop = await this._shopRepository.GetShopProfileByAccountIdAsync(
            this._currentPrincipalService.CurrentPrincipalId.Value).ConfigureAwait(false);
        try
        {
            var shopUpdated = this.UpdateShopInfomation(shop, request);
            this._shopRepository.Update(shopUpdated);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            var shopResponse = this._mapper.Map<ShopProfileResponse>(shopUpdated);
            return Result.Success(shopResponse);
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Cập nhật thông tin shop thất bại"));
        }
    }

    public Domain.Entities.Shop UpdateShopInfomation(Domain.Entities.Shop shop, UpdateInfoShopCommand info)
    {
        shop.Name = info.Name;
        shop.Description = info.Description;
        shop.PhoneNumber = info.PhoneNumber;
        shop.ActiveFrom = info.ActiveFrom;
        shop.ActiveTo = info.ActiveTo;
        shop.Building.Name = info.Address;
        shop.Building.Longitude = info.Longitude;
        shop.Building.Latitude = info.Latitude;
        shop.MinimumValueOrderFreeship = info.MinimumValueOrderFreeShip;
        shop.ShippingFee = info.ShippingFee;
        return shop;
    }
}