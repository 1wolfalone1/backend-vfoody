using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Application.UseCases.Shops.Commands.GetBan;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.GetUnBan;

public class ShopGetUnBanHandler : ICommandHandler<ShopGetUnBanCommand, Result>
{
    private readonly IEmailService _emailService;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ShopGetBanHandler> _logger;
    private readonly IShopRepository _shopRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IFirebaseNotificationService _firebaseNotificationService;

    public ShopGetUnBanHandler(IEmailService emailService, IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<ShopGetBanHandler> logger, IShopRepository shopRepository, INotificationRepository notificationRepository, IFirebaseNotificationService firebaseNotificationService)
    {
        _emailService = emailService;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _shopRepository = shopRepository;
        _notificationRepository = notificationRepository;
        _firebaseNotificationService = firebaseNotificationService;
    }

    public async Task<Result<Result>> Handle(ShopGetUnBanCommand request, CancellationToken cancellationToken)
    {
        var shop = this._shopRepository.GetById(request.ShopId);
        var account = this._accountRepository.GetById(shop.AccountId);
        if (shop == default)
            throw new InvalidBusinessException($"Không tìm thấy cửa hàng với id: {request.ShopId}");
        
        if (shop.Status == (int)ShopStatus.UnActive)
            throw new InvalidBusinessException($"Cửa hàng chưa được xác nhận nên không thể bỏ cấm");
        
        if (shop.Status != (int)ShopStatus.Ban)
            throw new InvalidBusinessException($"Cửa hàng này đang không bị cấm");

        if (account.Status == (int)AccountStatus.Ban)
            throw new InvalidBusinessException(
                $"Tài khoản của cửa hàng đang bị cấm nên không thể bỏ cấm cừa hàng {shop.Name}");

        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            if (this.SendEmailAnnounce(account.LastName, account.Email, request.Reason, shop.Name))
            {
                shop.Status = (int)ShopStatus.Active;
                this._shopRepository.Update(shop);
                await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
                
                // Send noti to customer    
                await this.SendNotificationAsync(shop.LogoUrl,
                    account.Id,
                    account.DeviceToken,
                    NotificationMessageConstants.Un_Ban_Shop_Title,
                    NotificationMessageConstants.Un_Ban_Shop_Cus_Content,
                    (int)Domain.Enums.Roles.Customer).ConfigureAwait(false);
                
                // Send noti to shop
                await this.SendNotificationAsync(shop.LogoUrl,
                    account.Id,
                    account.DeviceToken,
                    NotificationMessageConstants.Un_Ban_Shop_Title,
                    NotificationMessageConstants.Un_Ban_Shop_Content,
                    (int)Domain.Enums.Roles.Shop).ConfigureAwait(false);

                return Result.Success($"Bỏ cấm cửa hàng {shop.Name} của khách hàng {account.LastName} thành công");
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
                <p>Chúng tôi rất vui mừng thông báo rằng cửa hàng của bạn, " + shopName + @", đã được mở khóa và bạn có thể tiếp tục sử dụng dịch vụ của VFoody.</p>
                <p>Lý do: " + reason + @"</p>
                <p>Ngày hiệu lực: " + StringUtils.DateToStringFormat(DateTime.Now) + @"</p>
                <p>Chúng tôi cảm ơn sự kiên nhẫn của bạn trong thời gian cửa hàng bị cấm. Nếu bạn có bất kỳ câu hỏi nào hoặc cần thêm hỗ trợ, vui lòng liên hệ với đội hỗ trợ của chúng tôi.</p>
                <p>Chúc bạn kinh doanh thành công!</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ VFoody</p>
                " + EmailConstants.Staff_Support_Infor +
            @"</body>
        </html>";
        return this._emailService.SendEmail(email, "VFoody - Cửa hàng của bạn đả được bỏ cấm", emailBody);
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