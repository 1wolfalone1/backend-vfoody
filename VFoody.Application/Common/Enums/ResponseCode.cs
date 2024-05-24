using System.ComponentModel;

namespace VFoody.Application.Common.Enums;

public enum ResponseCode
{
    [Description("Common Error")] CommonError = 1,

    [Description("Validation Error")] ValidationError = 2,

    [Description("Mapping Error")] MappingError = 3,

    [Description("Unauthorized")] Unauthorized = 4,
    
    // Auth
        
    [Description("Invalid username or password")] AuthErrorInvalidUsernameOrPassword = 20,
    
    [Description("Invalid refresh token")] AuthErrorInvalidRefreshToken = 21,
}
