using AutoMapper;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Promotion.Models;
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
                    var response1 = this._mapper.Map<AllPromotionResponse>(this.UpdatePersonPromotion(request.Promotion));
                    response1.PromotionType = PromotionTypes.PersonPromotion;
                    await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
                    return Result.Success(response1);
                case PromotionTypes.PlatformPromotion:
                    var response2 = this._mapper.Map<AllPromotionResponse>(this.UpdatePlatformPromotion(request.Promotion));
                    response2.PromotionType = PromotionTypes.PlatformPromotion;
                    await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
                    return Result.Success(response2);
                case PromotionTypes.ShopPromotion:
                    var response3 = this._mapper.Map<AllPromotionResponse>(this.UpdateShopPromotion(request.Promotion));
                    response3.PromotionType = PromotionTypes.ShopPromotion;
                    await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
                    return Result.Success(response3);
            }

            throw new InvalidBusinessException($"Cập nhật với id:{request.Promotion.Id} thất bại");
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw;
        }
    }

    private PlatformPromotion UpdatePlatformPromotion(UpdatePromotionInfoRequest promotion)
    {
        var platformPromotion = this._platformPromotionRepository.GetById(promotion.Id);
        platformPromotion.Title = promotion.Title;
        platformPromotion.Description = promotion.Description;
        platformPromotion.BannerUrl = promotion.BannerUrl;
        platformPromotion.StartDate = promotion.StartDate;
        platformPromotion.ApplyType = (int)promotion.ApplyType;
        platformPromotion.AmountRate = promotion.AmountRate;
        platformPromotion.AmountValue = promotion.AmountValue;
        platformPromotion.MinimumOrderValue = promotion.MinimumOrderValue;
        platformPromotion.MaximumApplyValue = promotion.MaximumApplyValue;
        platformPromotion.UsageLimit = promotion.UsageLimit;
        platformPromotion.Status = (int)promotion.Status;
        this._platformPromotionRepository.Update(platformPromotion);
        return platformPromotion;
    }
    
    private PersonPromotion UpdatePersonPromotion(UpdatePromotionInfoRequest promotion)
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
        return personPromotion;
    }
    
    private ShopPromotion UpdateShopPromotion(UpdatePromotionInfoRequest promotion)
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
        return shopPromotion;
    }
}