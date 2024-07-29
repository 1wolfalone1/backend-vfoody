using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Commands.SendMail;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.SendMail;

public class SendMailHandler : ICommandHandler<SendMailCommand, Result>
{
    private readonly ILogger<SendMailHandler> _logger;
    private readonly IEmailService _emailService;

    public SendMailHandler(
        ILogger<SendMailHandler> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<Result<Result>> Handle(SendMailCommand request, CancellationToken cancellationToken)
    {
        List<string> emails = new List<string>();
        emails.Add("ducdmdse160104@fpt.edu.vn");
        emails.Add("hoangtienbmt2911@gmail.com");
        emails.Add("thientryhard@gmail.com");
        emails.Add("phuothuynh2002@gmail.com");
        emails.Add("camtrungtennha@gmail.com");
        emails.Add("thongnvse160162@fpt.edu.vn");
        emails.ForEach(email =>
        {
            _emailService.SendEmail(email, "Có điểm MLN mấy con ĐĨ", "Có điểm MLN mấy con ĐĨ");
        });

        return Result.Success("Send Mail successfully.");
    }
}