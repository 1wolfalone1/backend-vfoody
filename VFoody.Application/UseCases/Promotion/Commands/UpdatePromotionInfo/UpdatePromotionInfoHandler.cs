using AutoMapper;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.UpdatePromotionInfo;

public class UpdatePromotionInfoHandler : ICommandHandler<UpdatePromotionInfoCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonPromotionRepository _personPromotionRepository;
    private readonly IPlatformPromotionRepository _platformPromotionRepository;
    private readonly IShopPromotionRepository _shopPromotionRepository;
    private readonly ILogger<UpdatePromotionInfoHandler> _logger;
    private readonly IMapper _mapper;

    public UpdatePromotionInfoHandler(IUnitOfWork unitOfWork, IPersonPromotionRepository personPromotionRepository, IPlatformPromotionRepository platformPromotionRepository, IShopPromotionRepository shopPromotionRepository, ILogger<UpdatePromotionInfoHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _personPromotionRepository = personPromotionRepository;
        _platformPromotionRepository = platformPromotionRepository;
        _shopPromotionRepository = shopPromotionRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(UpdatePromotionInfoCommand request, CancellationToken cancellationToken)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            switch (request.Promotion.PromotionType)
            {
                case PromotionTypes.PersonPromotion:
                    this.UpdatePersonPromotion(request.Promotion);
                    break;
                case PromotionTypes.PlatformPromotion:
                    this.UpdatePlatformPromotion(request.Promotion);
                    break;
                case PromotionTypes.ShopPromotion:
                    this.UpdateShopPromotion(request.Promotion);       
                    break;
            }

            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Success($"Cập nhật khuyến mãi với id: {request.Promotion.Id} thành công");
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw;
        }
    }

    private async Task UpdatePlatformPromotion(UpdatePromotionInfoRequest promotion)
    {
        var platformPromotion = this._platformPromotionRepository.GetById(promotion.Id);
        platformPromotion.Title = promotion.Title;
        platformPromotion.Description = promotion.Description;
        platformPromotion.StartDate = promotion.StartDate;
        platformPromotion.ApplyType = (int)promotion.ApplyType;
        platformPromotion.AmountRate = promotion.AmountRate;
        platformPromotion.AmountValue = promotion.AmountValue;
        platformPromotion.MinimumOrderValue = promotion.MinimumOrderValue;
        platformPromotion.MaximumApplyValue = promotion.MaximumApplyValue;
        platformPromotion.UsageLimit = promotion.UsageLimit;
        platformPromotion.Status = (int)promotion.Status;
        this._platformPromotionRepository.Update(platformPromotion);
    }
    
    private async Task UpdatePersonPromotion(UpdatePromotionInfoRequest promotion)
    {
        var personPromotion = this._personPromotionRepository.GetById(promotion.Id);
        personPromotion.Title = promotion.Title;
        personPromotion.Description = promotion.Description;
        personPromotion.StartDate = promotion.StartDate;
        personPromotion.ApplyType = (int)promotion.ApplyType;
        personPromotion.AmountRate = promotion.AmountRate;
        personPromotion.AmountValue = promotion.AmountValue;
        personPromotion.MinimumOrderValue = promotion.MinimumOrderValue;
        personPromotion.MaximumApplyValue = promotion.MaximumApplyValue;
        personPromotion.UsageLimit = promotion.UsageLimit;
        personPromotion.Status = (int)promotion.Status;
        this._personPromotionRepository.Update(personPromotion);
    }
    
    private async Task UpdateShopPromotion(UpdatePromotionInfoRequest promotion)
    {
        var shopPromotion = this._shopPromotionRepository.GetById(promotion.Id);
        shopPromotion.Title = promotion.Title;
        shopPromotion.Description = promotion.Description;
        shopPromotion.StartDate = promotion.StartDate;
        shopPromotion.ApplyType = (int)promotion.ApplyType;
        shopPromotion.AmountRate = promotion.AmountRate;
        shopPromotion.AmountValue = promotion.AmountValue;
        shopPromotion.MinimumOrderValue = promotion.MinimumOrderValue;
        shopPromotion.MaximumApplyValue = promotion.MaximumApplyValue;
        shopPromotion.UsageLimit = promotion.UsageLimit;
        shopPromotion.Status = (int)promotion.Status;
        this._shopPromotionRepository.Update(shopPromotion);
    }
}