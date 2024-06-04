using System.ComponentModel;

namespace VFoody.Application.Common.Enums;

public enum ResponseCode
{
    [Description("Common Error")] CommonError = 1,

    [Description("Validation Error")] ValidationError = 2,

    [Description("Mapping Error")] MappingError = 3,

    [Description("Unauthorized")] Unauthorized = 4,
    
    // Auth
        
    [Description("Email hoặc Mật Khẩu không chính xác")] AuthErrorInvalidUsernameOrPassword = 20,
    
    [Description("Invalid refresh token")] AuthErrorInvalidRefreshToken = 21,
    
    //Very
    [Description("Tài khoản của bạn chưa được xác thực")] VerifyErrorInvalidAccount = 31,
    
    //Ban
    [Description("Tài khoản bạn đã bị khóa")] BanErrorAccount = 41,
}
