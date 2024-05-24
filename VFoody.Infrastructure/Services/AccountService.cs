using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;

namespace VFoody.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> logger;

    public AccountService(ILogger<AccountService> logger)
    {
        this.logger = logger;
    }

    public void TestWriteLog()
    {
        this.logger.LogInformation($"password: {BCrypUnitls.Hash("1")}");
        this.logger.LogInformation("Log successly");
    }
}
