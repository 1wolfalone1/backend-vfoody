using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.AdminRejectShopWithdrawal;

public class AdminRejectShopWithdrawalHandler : ICommandHandler<AdminRejectShopWithdrawalCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShopRepository _shopRepository;
    private readonly IShopBalanceHistoryRepository _historyRepository;
    private readonly IShopWithdrawalRequestRepository _withdrawalRequestRepository;
    private readonly IFirebaseNotificationService _firebaseNotification;
    private readonly ILogger<AdminRejectShopWithdrawalHandler> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly IEmailService _emailService;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;

    public AdminRejectShopWithdrawalHandler(IUnitOfWork unitOfWork, IShopRepository shopRepository, IShopBalanceHistoryRepository historyRepository, IShopWithdrawalRequestRepository withdrawalRequestRepository, IFirebaseNotificationService firebaseNotification, ILogger<AdminRejectShopWithdrawalHandler> logger, INotificationRepository notificationRepository, IEmailService emailService, IAccountRepository accountRepository, ICurrentPrincipalService currentPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _shopRepository = shopRepository;
        _historyRepository = historyRepository;
        _withdrawalRequestRepository = withdrawalRequestRepository;
        _firebaseNotification = firebaseNotification;
        _logger = logger;
        _notificationRepository = notificationRepository;
        _emailService = emailService;
        _accountRepository = accountRepository;
        _currentPrincipalService = currentPrincipalService;
    }

    public async Task<Result<Result>> Handle(AdminRejectShopWithdrawalCommand request, CancellationToken cancellationToken)
    {
        Validate(request);
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            await this.UpdateShopRequestAsync(request.RequestId, request.ShopId, request.Reason).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);

            // Send notification
            var shopAccount = this._shopRepository.GetAccountByShopId(request.ShopId);
            var shop = this._shopRepository.GetById(request.ShopId);
            var requestWith = this._withdrawalRequestRepository.GetById(request.RequestId);
            await SendNotificationAsync(NotificationMessageConstants.Noti_Logo_Default_Url,
                shopAccount.Id,
                shopAccount.DeviceToken,
                NotificationMessageConstants.Shop_Withdrawal_Title,
                string.Format(NotificationMessageConstants.Shop_Withdrawal_Reject_Content,
                    StringUtils.DateToStringFormat(DateTime.Now)),
                (int)Domain.Enums.Roles.Shop
            ).ConfigureAwait(false);
            
            await SendNotificationAsync(NotificationMessageConstants.Noti_Logo_Default_Url,
                shopAccount.Id,
                shopAccount.DeviceToken,
                NotificationMessageConstants.Shop_Withdrawal_Title,
                string.Format(NotificationMessageConstants.Shop_Withdrawal_Reject_Refund_Content,
                    StringUtils.ToVnCurrencyFormat(requestWith.RequestedAmount)),
                (int)Domain.Enums.Roles.Shop
            ).ConfigureAwait(false);
            
            // Send email
            SendWithdrawRejectionEmail(shop.Name, shopAccount.Email, requestWith.RequestedAmount, request.Reason);
            return Result.Success("Từ chối yêu cầu rút tiền thành công");
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Từ chối yêu cầu rút tiền thất bại"));
        }
    }

    private void Validate(AdminRejectShopWithdrawalCommand request)
    {
        var shop = this._shopRepository.GetById(request.ShopId);
        if (shop == default)
            throw new InvalidBusinessException($"Không tìm thầy cửa hàng với id: {request.ShopId}");

        var requestWith = this._withdrawalRequestRepository.GetById(request.RequestId);
        if (requestWith == default)
            throw new InvalidBusinessException($"Không tìm thấy yêu cầu rút tiền với id: {request.RequestId}");

        if (requestWith.Status != (int)ShopWithdrawalRequestStatus.Pending)
            throw new InvalidBusinessException("Yêu cầu đang không trong trạng thái có thể từ chối");
    }

    private async Task UpdateShopRequestAsync(int requestId, int shopId, string reason)
    {
        var request = this._withdrawalRequestRepository.GetById(requestId);
        request.Status = (int)ShopWithdrawalRequestStatus.Rejected;
        request.ProcessedDate = DateTime.Now;
        request.Note = reason;
        request.ProcessedBy = this._currentPrincipalService.CurrentPrincipalId.Value;
        
        // Update shop balance
        var shop = this._shopRepository.GetById(shopId);
        var shopBalanceHistory = new ShopBalanceHistory()
        {
            ShopId = shopId,
            ChangeAmount = request.RequestedAmount,
            BalanceBeforeChange = shop.Balance,
            BalanceAfterChange = shop.Balance + request.RequestedAmount,
            TransactionType = (int)ShopBalanceTransactionTypes.ManualAdjustment,
            Description = string.Format(ShopBalanceHistoryConstants.Shop_Request_Get_Reject, StringUtils.ToVnCurrencyFormat(request.RequestedAmount))
        };
        
        shop.Balance += request.RequestedAmount;

        await this._historyRepository.AddAsync(shopBalanceHistory);
        this._shopRepository.Update(shop);
        this._withdrawalRequestRepository.Update(request);
    }
    
    private async Task SendNotificationAsync(string imageUrl, int accountId, string deviceToken, string title, string content, int role)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            this._firebaseNotification.SendNotification(deviceToken, title, content, imageUrl);
            Notification noti = new Notification()
            {
                AccountId = accountId,
                Readed = 0,
                Title = title,
                Content = content,
                ImageUrl = imageUrl,
                RoleId = role,
            };
            await this._notificationRepository.AddAsync(noti).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
        }
    }
    
    private bool SendWithdrawRejectionEmail(string storeName, string storeEmail, float amount, string reason)
    {
        string emailBody = @"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='margin-bottom: 20px; text-align: center;'>
                    <img src='https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1717170673218-42e3e4c6-ff37-4810-b6ab-860551bba3b7' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                </div>
                <p>Xin chào <strong>" + storeName + @"</strong>,</p>
                <p>Chúng tôi rất tiếc thông báo rằng yêu cầu rút số tiền <strong>" + StringUtils.ToVnCurrencyFormat(amount) + @"</strong> từ cửa hàng của bạn đã bị từ chối.</p>
                <p>Lý do từ chối: " + reason + @"</p>
                <p>Nếu bạn có bất kỳ câu hỏi nào hoặc cần thêm hỗ trợ, vui lòng liên hệ với đội hỗ trợ của chúng tôi.</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ VFoody</p>
                " + EmailConstants.Staff_Support_Infor +
            @"</body>
        </html>";
    
        return this._emailService.SendEmail(storeEmail, "VFoody - Yêu cầu rút tiền bị từ chối", emailBody);
    }

}