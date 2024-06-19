using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.CreatePromotion;

public class CreatePromotionHandler : ICommandHandler<CreatePromotionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlatformPromotionRepository _platformPromotionRepository;
    private readonly IPersonPromotionRepository _personPromotionRepository;
    private readonly IShopPromotionRepository _shopPromotionRepository;
    private readonly ILogger<CreatePromotionHandler> _logger;

    public CreatePromotionHandler(IUnitOfWork unitOfWork, IPlatformPromotionRepository platformPromotionRepository, IPersonPromotionRepository personPromotionRepository, ILogger<CreatePromotionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _platformPromotionRepository = platformPromotionRepository;
        _personPromotionRepository = personPromotionRepository;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var result = new BaseEntity();
        switch (request.CreatePromotion.PromotionType)
        {
            case PromotionTypes.PlatformPromotion:
                result = await this.CreatePlatformPromotionAsync(request.CreatePromotion).ConfigureAwait(false);
                result = result as PlatformPromotion;
                break;
            case PromotionTypes.PersonPromotion:
                result= await this.CreatePersonPromotionAsync(request.CreatePromotion).ConfigureAwait(false);
                result = result as PersonPromotion;
                break;
            case PromotionTypes.ShopPromotion:
                result = await this.CreateShopPromotionAsync(request.CreatePromotion).ConfigureAwait(false);
                result = result as ShopPromotion;
                break;
            default:
                result = null;
                break;
        }

        if (result != null)
        {
            return Result.Create(result );
        }

        return Result.Failure(new Error("500", "Không thể tạo promotion"));
    }

    private async Task<PlatformPromotion> CreatePlatformPromotionAsync(CreatePromotionRequest promotion)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var platformPromotion = new PlatformPromotion
            {
                Title = promotion.Title,
                BannerUrl = promotion.BannerUrl,
                AmountRate = promotion.AmountRate,
                AmountValue = promotion.AmountValue,
                MinimumOrderValue = promotion.MinimumOrderValue,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                UsageLimit = promotion.UsageLimit,
                NumberOfUsed = 0,
                ApplyType = (int)promotion.ApplyType,
                Status = (int)PlatformPromotionStatus.Active,
                Description = promotion.Description
            };
            await this._platformPromotionRepository.AddAsync(platformPromotion).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return platformPromotion;
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();;
            this._logger.LogError(e, e.Message);
            return null;
        }
    }
    
    private async Task<PersonPromotion> CreatePersonPromotionAsync(CreatePromotionRequest promotion)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var personPromotion = new PersonPromotion()
            {
                Title = promotion.Title,
                AmountRate = promotion.AmountRate,
                AmountValue = promotion.AmountValue,
                MinimumOrderValue = promotion.MinimumOrderValue,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                UsageLimit = promotion.UsageLimit,
                NumberOfUsed = 0,
                ApplyType = (int)promotion.ApplyType,
                Status = (int)PlatformPromotionStatus.Active,
                AccountId = promotion.AccountId.Value,
                Description = promotion.Description
            };
            await this._personPromotionRepository.AddAsync(personPromotion).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return personPromotion;
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            return null;
        }
    }
    
    private async Task<ShopPromotion> CreateShopPromotionAsync(CreatePromotionRequest promotion)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var shopPromotion = new ShopPromotion()
            {
                Title = promotion.Title,
                AmountRate = promotion.AmountRate,
                AmountValue = promotion.AmountValue,
                MinimumOrderValue = promotion.MinimumOrderValue,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                UsageLimit = promotion.UsageLimit,
                NumberOfUsed = 0,
                ApplyType = (int)promotion.ApplyType,
                Status = (int)PlatformPromotionStatus.Active,
                ShopId = promotion.ShopId.Value,
                Description = promotion.Description
            };
            await this._shopPromotionRepository.AddAsync(shopPromotion).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return shopPromotion;
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();;
            this._logger.LogError(e, e.Message);
            return null;
        }
    }
}