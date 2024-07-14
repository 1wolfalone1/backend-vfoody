using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.BanAction;

public class BanActionHandler : ICommandHandler<BanActionCommand, Result>
{
    private readonly IEmailService _emailService;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BanActionHandler> _logger;

    public BanActionHandler(IEmailService emailService, IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<BanActionHandler> logger)
    {
        _emailService = emailService;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(BanActionCommand request, CancellationToken cancellationToken)
    {
        var account = this._accountRepository.GetById(request.AccountId);
        if (account == default)
            throw new InvalidBusinessException($"Không tìm thấy tài khoản với id là {account.Id}");
        
        if (account.RoleId != (int)Domain.Enums.Roles.Customer)
            throw new InvalidBusinessException($"Tài khoản này không thể cấm với hành động này");

        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            if (this.SendEmailAnnounce(account.LastName, account.Email, request.Reason))
            {
                account.Status = (int)AccountStatus.Ban;
                this._accountRepository.Update(account);
                await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
                return Result.Success($"Cấm tài khoản của khách hàng {account.LastName} thành công");
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

    private bool SendEmailAnnounce(string fullName, string email, string reason)
    {
        string emailBody = @"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='margin-bottom: 20px; text-align: center;'>
                    <img src='https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1717170673218-42e3e4c6-ff37-4810-b6ab-860551bba3b7' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                </div>
                <p>Xin chào " + fullName + @",</p>
                <p>Chúng tôi rất tiếc phải thông báo rằng tài khoản của bạn đã bị cấm vì lý do sau:</p>
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
        return this._emailService.SendEmail(email, "VFoody - Tài khoản của bạn bị cấm", emailBody);
    }
}