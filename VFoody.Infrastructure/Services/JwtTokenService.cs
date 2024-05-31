using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;
using VFoody.Infrastructure.Settings;

namespace VFoody.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService, IBaseService
{
    private JwtSetting _jwtSetting;
    private IRoleRepository _roleRepository;
    public JwtTokenService(IConfiguration configuration, JwtSetting jwtSetting, IRoleRepository roleRepository)
    {
        _jwtSetting = jwtSetting;
        _roleRepository = roleRepository;
    }
    
    private string GenerateJwtToken(Account account, int expireInMinutes)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var role = this._roleRepository.GetById(account.RoleId)
                       ?? throw new UnauthorizedAccessException($"Role id of account id: {account.Id} not correct");
        var claims = new[]
        {
            new Claim(ClaimTypes.Sid, account.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, account.Email),
            // new Claim(ClaimTypes.Name, account.FirstName),
            new Claim(ClaimTypes.Role, role.Name)
        };

        var token = new JwtSecurityToken(
            _jwtSetting.Issuer,
            _jwtSetting.Audience,
            claims,
            expires: DateTime.UtcNow.AddHours(expireInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public string GenerateJwtToken(Account account)
    {
        return GenerateJwtToken(account, _jwtSetting.TokenExpire);
    }
    
    public string GenerateJwtRefreshToken(Account account)
    {
        return GenerateJwtToken(account, _jwtSetting.RefreshTokenExpire);
    }
}