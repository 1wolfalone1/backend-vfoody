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

namespace VFoody.Application.UseCases.Shops.Commands.GetBan;

public class ShopGetBanHandler : ICommandHandler<ShopGetBanCommand, Result>
{
    private readonly IEmailService _emailService;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ShopGetBanHandler> _logger;
    private readonly IShopRepository _shopRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IFirebaseNotificationService _firebaseNotificationService;

    public ShopGetBanHandler(ILogger<ShopGetBanHandler> logger, IUnitOfWork unitOfWork, IAccountRepository accountRepository, IEmailService emailService, IShopRepository shopRepository, INotificationRepository notificationRepository, IFirebaseNotificationService firebaseNotificationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _emailService = emailService;
        _shopRepository = shopRepository;
        _notificationRepository = notificationRepository;
        _firebaseNotificationService = firebaseNotificationService;
    }

    public async Task<Result<Result>> Handle(ShopGetBanCommand request, CancellationToken cancellationToken)
    {
        var shop = this._shopRepository.GetById(request.ShopId);
        var account = this._accountRepository.GetById(shop.AccountId);
        if (shop == default)
            throw new InvalidBusinessException($"Không tìm thấy cửa hàng với id: {request.ShopId}");
        
        if (shop.Status == (int)ShopStatus.UnActive)
            throw new InvalidBusinessException($"Cửa hàng chưa được xác nhận nên không thể cấm");

        if (account.Status == (int)AccountStatus.Ban)
            throw new InvalidBusinessException(
                $"Tài khoản của cửa hàng đang bị cấm nên không thể ban cừa hàng {shop.Name}");

        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            if (this.SendEmailAnnounce(account.LastName, account.Email, request.Reason, shop.Name))
            {
                shop.Status = (int)ShopStatus.Ban;
                this._shopRepository.Update(shop);
                await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
                
                // Send noti to customer    
                await this.SendNotificationAsync(shop.LogoUrl,
                    account.Id,
                    account.DeviceToken,
                    NotificationMessageConstants.Ban_Shop_Title,
                    NotificationMessageConstants.Ban_Shop_Content,
                    (int)Domain.Enums.Roles.Customer).ConfigureAwait(false);
                return Result.Success($"Cấm cửa hàng {shop.Name} của khách hàng {account.LastName} thành công");
            }

            return Result.Failure(new Error("400",
                "Tạm thời dịch vụ gửi mail thông báo đang bảo trì vui lòng thử lại hành động này sau"));
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            throw;
        }
    }

    private bool SendEmailAnnounce(string fullName, string email, string reason, string shopName)
    {
        string emailBody = @"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='margin-bottom: 20px; text-align: center;'>
                    <img src='https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1717170673218-42e3e4c6-ff37-4810-b6ab-860551bba3b7' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                </div>
                <p>Xin chào " + fullName + @",</p>
                <p>Chúng tôi rất tiếc phải thông báo rằng cửa hàng của bạn, " + shopName + @", đã bị cấm vì lý do sau:</p>
                <div style='text-align: center; margin: 20px;'>
                    <span style='font-size: 18px; padding: 10px; border: 1px solid #ccc;'>" + reason + @"</span>
                </div>
                <p>Ngày hiệu lực: " + StringUtils.DateToStringFormat(DateTime.Now) + @"</p>
                <p>Nếu bạn cho rằng đây là một sai lầm hoặc cần thêm thông tin, vui lòng liên hệ với đội hỗ trợ của chúng tôi.</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ VFoody</p>
                " + EmailConstants.Staff_Support_Infor +
            @"</body>
        </html>";
        return this._emailService.SendEmail(email, "VFoody - Cửa hàng của bạn bị cấm", emailBody);
    }
    
    private async Task SendNotificationAsync(string imageUrl, int accountId, string deviceToken, string title, string content, int role)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            this._firebaseNotificationService.SendNotification(deviceToken, title, content, imageUrl);
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
}