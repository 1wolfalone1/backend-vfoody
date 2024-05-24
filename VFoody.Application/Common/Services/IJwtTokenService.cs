using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Services;

public interface IJwtTokenService
{
    string GenerateJwtToken(Account account);

    string GenerateJwtRefreshToken(Account account);
}