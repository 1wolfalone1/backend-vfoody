using System.Globalization;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Commands.ShopWithdrawalRequests;

public class ShopWithdrawalRequestHandler : ICommandHandler<ShopWithdrawalRequestCommand, Result>
{
    private readonly IFirebaseNotificationService _firebaseNotification;
    private readonly IEmailService _emailService;
    private readonly IShopWithdrawalRequestRepository _withdrawalRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ShopWithdrawalRequestHandler> _logger;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly INotificationRepository _notificationRepository;
    private readonly IShopRepository _shopRepository;
    private readonly IShopBalanceHistoryRepository _balanceHistoryRepository;

    public ShopWithdrawalRequestHandler(IFirebaseNotificationService firebaseNotificationService, IEmailService emailService, IShopWithdrawalRequestRepository withdrawalRequestRepository, IUnitOfWork unitOfWork, ILogger<ShopWithdrawalRequestHandler> logger, ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository, ICurrentAccountService currentAccountService, INotificationRepository notificationRepository, IShopBalanceHistoryRepository balanceHistoryRepository)
    {
        _firebaseNotification = firebaseNotificationService;
        _emailService = emailService;
        _withdrawalRequestRepository = withdrawalRequestRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
        _currentAccountService = currentAccountService;
        _notificationRepository = notificationRepository;
        _balanceHistoryRepository = balanceHistoryRepository;
    }

    public async Task<Result<Result>> Handle(ShopWithdrawalRequestCommand request, CancellationToken cancellationToken)
    {
        var shop = await this._shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId.Value);
        var shopAccount = _currentAccountService.GetCurrentAccount();
        if (shop == default)
            throw new InvalidBusinessException($"Cửa hàng bạn không còn tồn tại để thực hiện hành động này");

        this.Validate(request, shop);
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            await this.CreateShopWithdrawalAsync(request, shop).ConfigureAwait(false);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            // Send an email for System staff
            this.SendWithdrawRequestEmail(shop.Name, "vanthong07012002@gmail.com", request.CommandRequestModel.RequestedAmount);
            
            // Send an email for store
            this.SendWithdrawAcknowledgementEmail(shop.Name, shopAccount.Email,
                request.CommandRequestModel.RequestedAmount);
            
            // Send an notification for shop
            this.SendNotificationAsync(NotificationMessageConstants.Noti_Logo_Default_Url,
                shopAccount.Id,
                shopAccount.DeviceToken,
                NotificationMessageConstants.Shop_Withdrawal_Title,
                string.Format(NotificationMessageConstants.Shop_Withdrawal_Content,
                    StringUtils.ToVnCurrencyFormat(request.CommandRequestModel.RequestedAmount)),
                (int)Domain.Enums.Roles.Shop).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            throw new InvalidBusinessException($"Yêu cầu rút tiền thất bại");
        }

        return Result.Success("Tạo yêu cầu rút tiền thành công");
    }

    private void Validate(ShopWithdrawalRequestCommand request, Domain.Entities.Shop shop)
    {
        if (request.CommandRequestModel.RequestedAmount > shop.Balance)
            throw new InvalidBusinessException(
                $"Số dư của bạn hiện tại là {shop.Balance} không đủ để thực hiện yêu cầu rút {request.CommandRequestModel.RequestedAmount}");
    }

    private async Task<ShopWithdrawalRequest> CreateShopWithdrawalAsync(ShopWithdrawalRequestCommand request, Domain.Entities.Shop shop)
    {
        ShopWithdrawalRequest shopRequest = new ShopWithdrawalRequest
        {
            ShopId = shop.Id,
            RequestedAmount = request.CommandRequestModel.RequestedAmount,
            Status = (int)ShopWithdrawalRequestStatus.Pending,
            BankCode = request.CommandRequestModel.BankCode,
            BankShortName = request.CommandRequestModel.BankShortName,
            BankAccountNumber = request.CommandRequestModel.BankAccountNumber,
            RequestedDate = new DateTime(),
            ProcessedDate = null,
            ProcessedBy = null,
        };

        ShopBalanceHistory shopBalance = new ShopBalanceHistory
        {
            ShopId = shop.Id,
            ChangeAmount = -request.CommandRequestModel.RequestedAmount,
            BalanceBeforeChange = shop.Balance,
            BalanceAfterChange = shop.Balance - request.CommandRequestModel.RequestedAmount,
            TransactionType = (int)ShopBalanceTransactionTypes.ManualAdjustment,
            Description = string.Format(ShopBalanceHistoryConstants.Shop_Request_Withdrawal,
                StringUtils.ToVnCurrencyFormat(request.CommandRequestModel.RequestedAmount))
        };
        
        shop.Balance -= request.CommandRequestModel.RequestedAmount;
        this._shopRepository.Update(shop);
        await this._balanceHistoryRepository.AddAsync(shopBalance).ConfigureAwait(false);
        await this._withdrawalRequestRepository.AddAsync(shopRequest).ConfigureAwait(false);
        return shopRequest;
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

    private bool SendWithdrawRequestEmail(string storeName, string staffEmail, float amount)
    {
        string emailBody = @"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='margin-bottom: 20px; text-align: center;'>
                    <img src='https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1717170673218-42e3e4c6-ff37-4810-b6ab-860551bba3b7' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                </div>
                <p>Xin chào đội ngũ hệ thống,</p>
                <p>Cửa hàng <strong>" + storeName + @"</strong> muốn rút số tiền <strong>" + StringUtils.ToVnCurrencyFormat(amount) + @"</strong> </p>
                <p>Ngày yêu cầu: " + StringUtils.DateToStringFormat(DateTime.Now)+ @"</p>
                <p>Vui lòng xử lý yêu cầu này sớm nhất có thể.</p>
                <p>Trân trọng,</p>
                <p>Hệ thống VFoody</p>
                <p>Đội ngũ VFoody</p>
            </body>
        </html>";
    
        return this._emailService.SendEmail(staffEmail, "Yêu cầu rút tiền từ cửa hàng " + storeName, emailBody);
    }
    
    private bool SendWithdrawAcknowledgementEmail(string storeName, string storeEmail, float amount)
    {
        string emailBody = @"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='margin-bottom: 20px; text-align: center;'>
                    <img src='https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1717170673218-42e3e4c6-ff37-4810-b6ab-860551bba3b7' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                </div>
                <p>Xin chào <strong>" + storeName + @"</strong>,</p>
                <p>Chúng tôi đã nhận được yêu cầu rút số tiền <strong>" + StringUtils.ToVnCurrencyFormat(amount) + @"</strong> từ cửa hàng của bạn.</p>
                <p>Yêu cầu của bạn sẽ được xử lý trong thời gian 24 giờ kể từ ngày: " + StringUtils.DateToStringFormat(DateTime.Now) + @"</p>
                <p>Chúng tôi sẽ thông báo cho bạn khi quá trình rút tiền hoàn tất. Nếu bạn có bất kỳ câu hỏi nào hoặc cần thêm hỗ trợ, vui lòng liên hệ với đội hỗ trợ của chúng tôi.</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ VFoody</p>
                " + EmailConstants.Staff_Support_Infor +
            @"</body>
        </html>";
    
        return this._emailService.SendEmail(storeEmail, "VFoody - Xác nhận yêu cầu rút tiền", emailBody);
    }


}