using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Services;

public interface ICurrentAccountService
{
    public Account GetCurrentAccount();
}