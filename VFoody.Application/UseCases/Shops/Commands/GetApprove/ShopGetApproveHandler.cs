using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.GetApprove;

public class ShopGetApproveHandler : ICommandHandler<ShopGetApproveCommand, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IFirebaseNotificationService _firebaseNotification;
    private readonly ILogger<ShopGetApproveHandler> _logger;
    private readonly INotificationRepository _notificationRepository;

    public ShopGetApproveHandler(IShopRepository shopRepository, IUnitOfWork unitOfWork, IEmailService emailService, IFirebaseNotificationService firebaseNotification, IAccountRepository accountRepository, ILogger<ShopGetApproveHandler> logger, INotificationRepository notificationRepository)
    {
        _shopRepository = shopRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _firebaseNotification = firebaseNotification;
        _accountRepository = accountRepository;
        _logger = logger;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<Result>> Handle(ShopGetApproveCommand request, CancellationToken cancellationToken)
    {
        var shop = this._shopRepository.GetById(request.ShopId);
        var account = this._accountRepository.GetById(shop.AccountId);
        if (shop == default)
            throw new InvalidBusinessException($"Không tìm thấy cửa hàng với id: {request.ShopId}");
        
        if (shop.Status == (int)ShopStatus.Ban)
            throw new InvalidBusinessException($"Cửa hàng đang bị cấm nên không thể xác nhận");

        if (account.Status == (int)AccountStatus.Ban)
            throw new InvalidBusinessException(
                $"Tài khoản của cửa hàng đang bị cấm nên không thể xác nhận cừa hàng {shop.Name}");
        
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            if (this.SendEmailAnnounce(account.LastName, account.Email, shop.Name))
            {
                shop.Status = (int)ShopStatus.Active;
                this._shopRepository.Update(shop);
                await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
                
                // Send noti to account anncouce
                await this.SendNotificationAsync(shop.LogoUrl,
                    account.Id,
                    account.DeviceToken,
                    NotificationMessageConstants.Approve_Shop_Title,
                    string.Format(NotificationMessageConstants.Approve_Shop_Cus_Content, shop.Name),
                    (int)Domain.Enums.Roles.Customer
                ).ConfigureAwait(false);

                // Send noti to shop
                await this.SendNotificationAsync(shop.LogoUrl,
                    account.Id,
                    account.DeviceToken,
                    NotificationMessageConstants.Approve_Shop_Title,
                    NotificationMessageConstants.Apprve_Shop_Welcome,
                    (int)Domain.Enums.Roles.Shop
                ).ConfigureAwait(false);

                return Result.Success($"Xác nhận cửa hàng {shop.Name} thành công");
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
    
    private bool SendEmailAnnounce(string fullName, string email, string shopName)
    {
        string date = DateTime.Now.ToString("dd MMMM yyyy");
        string emailBody = @"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='margin-bottom: 20px; text-align: center;'>
                    <img src='https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1717170673218-42e3e4c6-ff37-4810-b6ab-860551bba3b7' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                </div>
                <p>Xin chào " + fullName + @",</p>
                <p>Chúng tôi rất vui mừng thông báo rằng yêu cầu nâng cấp tài khoản của bạn lên tài khoản cửa hàng cho " + shopName + @" đã được chấp thuận.</p>
                <p>Ngày hiệu lực: " + date + @"</p>
                <p>Bạn hiện có thể truy cập các tính năng và lợi ích dành riêng cho cửa hàng trên VFoody. Nếu bạn có bất kỳ câu hỏi nào hoặc cần thêm hỗ trợ, vui lòng liên hệ với đội hỗ trợ của chúng tôi.</p>
                <p>Chúc bạn kinh doanh thành công!</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ VFoody</p>
            </body>
        </html>";
        return this._emailService.SendEmail(email, "VFoody - Tài khoản của bạn nâng cấp thành cửa hàng thành công", emailBody);
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
}