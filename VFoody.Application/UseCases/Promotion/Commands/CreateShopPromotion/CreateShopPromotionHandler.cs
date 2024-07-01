using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Commands.CreateShopPromotion;

public class CreateShopPromotionHandler : ICommandHandler<CreateShopPromotionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShopPromotionRepository _shopPromotionRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateShopPromotionHandler> _logger;

    public CreateShopPromotionHandler(
        IUnitOfWork unitOfWork, IShopPromotionRepository shopPromotionRepository,
        ILogger<CreateShopPromotionHandler> logger, ICurrentPrincipalService currentPrincipalService,
        IMapper mapper, IShopRepository shopRepository
    )
    {
        _unitOfWork = unitOfWork;
        _shopPromotionRepository = shopPromotionRepository;
        _logger = logger;
        _currentPrincipalService = currentPrincipalService;
        _mapper = mapper;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(CreateShopPromotionCommand request, CancellationToken cancellationToken)
    {
        if (request.ApplyType == PromotionApplyTypes.Percent)
        {
            if (request.AmountRate <= 0)
            {
                throw new InvalidBusinessException("Phần trăm giảm giá phải lớn hơn 0.");
            }

            if (request.MaximumApplyValue <= 0)
            {
                throw new InvalidBusinessException("Số tiền giảm giá tối đa phải lớn hơn 0.");
            }
        }
        else
        {
            if (request.AmountValue <= 0)
            {
                throw new InvalidBusinessException("Số tiền giảm giá phải lớn hơn 0.");
            }
        }

        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        var entity = _mapper.Map<ShopPromotion>(request);
        entity.NumberOfUsed = 0;
        entity.ShopId = shop.Id;
        entity.Status = (int)PromotionStatus.Active;
        try
        {
            await _unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            await _shopPromotionRepository.AddAsync(entity);
            await _unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Create("Create shop promotion successfully.");
        }
        catch (Exception e)
        {
            //Rollback when exception
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }
}