using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Accounts.Commands.CheckAccount;
using VFoody.Application.UseCases.Accounts.Commands.CheckAuth.VerifyToken;
using VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;
using VFoody.Application.UseCases.Accounts.Commands.ForgotPasswordFirebase;
using VFoody.Application.UseCases.Accounts.Commands.Register;
using VFoody.Application.UseCases.Accounts.Commands.RegisterVerifyFirebase;
using VFoody.Application.UseCases.Accounts.Commands.SendCode;
using VFoody.Application.UseCases.Accounts.Commands.UpdateAccountDeviceToken;
using VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UpdateProfile;
using VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UploadAvatar;
using VFoody.Application.UseCases.Accounts.Commands.UpdateToShopOwner;
using VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;
using VFoody.Application.UseCases.Accounts.Commands.VerifyRegisterCode;
using VFoody.Application.UseCases.Accounts.Queries;
using VFoody.Application.UseCases.Accounts.Queries.AccountInfo;
using VFoody.Application.UseCases.Accounts.Queries.LoginToShop;
using VFoody.Application.UseCases.Accounts.Queries.ManageAccount;
using VFoody.Application.UseCases.Firebases.Commands.VerifyIdTokens;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.API.Controllers;

[Route("/api/v1/")]
public class AccountController : BaseApiController
{
    private readonly IMapper _mapper;

    public AccountController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost("customer/login")]
    public async  Task<IActionResult> Login(AccountLoginRequest loginRequest)
    {
        return this.HandleResult(await this.Mediator.Send(new CustomerLoginCommand
        {
            AccountLogin = loginRequest
        }));
    }

    [HttpPost("customer/register")]
    public async  Task<IActionResult> Register([FromBody] CustomerRegisterRequest registerRequest)
    {
        var command = _mapper.Map<CustomerRegisterCommand>(registerRequest);
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("customer/firebase/register")]
    public async  Task<IActionResult> Register([FromBody] RegisterFireBaseCommand registerFireBaseCommand)
    {
        return HandleResult(await Mediator.Send(registerFireBaseCommand));
    }

    [HttpPost("customer/forgot-password")]
    public async  Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
    {
        var command = _mapper.Map<ForgotPasswordCommand>(forgotPasswordRequest);
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("customer/forgot-password/verify")]
    public async  Task<IActionResult> VerifyCodeForgotPassword([FromBody] VerifyCodeForgotPasswordRequest verifyCodeForgotPasswordRequest)
    {
        var command = _mapper.Map<VerifyCodeForgotPasswordCommand>(verifyCodeForgotPasswordRequest);
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("customer/register/verify")]
    public async  Task<IActionResult> VerifyCodeRegister([FromBody] AccountVerifyRequest accountVerifyRequest)
    {
        var command = _mapper.Map<AccountVerifyCommand>(accountVerifyRequest);
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("customer/send-code")]
    public async  Task<IActionResult> SendCodeVerify([FromBody] AccountSendCodeRequest accountSendCodeRequest)
    {
        var command = _mapper.Map<AccountSendCodeCommand>(accountSendCodeRequest);
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("customer/google/login")]
    public async Task<IActionResult> LoginGoogle([FromBody] AccountGoogleLoginRequest accessToken)
    {
        return this.HandleResult(await this.Mediator.Send(new CustomerLoginGoogleCommand
            { AccessToken = accessToken.AccessToken }));
    }

    [HttpGet("customer")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> GetCustomerInfor()
    {
        return this.HandleResult(await this.Mediator.Send(new GetCustomerInforQuery()));
    }
    
    [HttpPost("firebase/verify")]
    public async Task<IActionResult> VerifyIdToken([FromBody] VerifyIDTokenRequest verifyIdTokenRequest)
    {
        return this.HandleResult(await this.Mediator.Send(new VerifyIDTokensCommand()
        {
            IdToken = verifyIdTokenRequest.IdToken
        }));
    }
    
    [HttpPost("customer/firebase/forgot-password")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async  Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordFirebaseRequest forgotPasswordFirebaseRequest)
    {
        return this.HandleResult(await this.Mediator.Send(new ForgotPasswordFirebaseCommand()
        {
            ForgotPasswordFirebaseRequest = forgotPasswordFirebaseRequest
        }));
    }

    [HttpPut("customer/profile/{id}")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> UpdateCustomerProfile([FromBody] UpdateProfileRequest updateProfileRequest, int id)
    {
        return this.HandleResult(await this.Mediator.Send(new UpdateProfileCommand
        {
            UpdateProfileRequest = updateProfileRequest,
            Id = id
        }));
    }
    
    [HttpPut("customer/upload/{id}")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> UpdateCustomerProfile(IFormFile avatarImageFile, int id)
    {
        return this.HandleResult(await this.Mediator.Send(new UpdateLoadAvatarCommand()
        {
            AvatarImageFile = avatarImageFile,
            Id = id
        }));
    }

    [HttpPost("customer/account/check")]
    public async  Task<IActionResult> CheckExistedAccount(CheckAccountCommand checkAccountCommand)
    {
        return HandleResult(await this.Mediator.Send(checkAccountCommand));
    }

    [HttpGet("admin/account/all")]
    [Authorize(Roles = $"{IdentityConst.AdminClaimName}")]
    public async Task<IActionResult> GetAllAccount(int pageIndex, int pageSize)
    {
        return HandleResult(await Mediator.Send(new GetAllAccountQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        }));
    }

    [HttpGet("admin/account/info")]
    [Authorize(Roles = $"{IdentityConst.AdminClaimName}")]
    public async Task<IActionResult> GetAllAccount(int accountId)
    {
        return HandleResult(await Mediator.Send(new GetAccountInfoQuery(accountId)));
    }

    [HttpPut("customer/account/device-token")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName},{IdentityConst.ShopClaimName}")]
    public async Task<IActionResult> UpdateAccount([FromBody] string deviceToken)
    {
        return this.HandleResult(await Mediator.Send(new UpdateAccountDeviceTokenCommand
        {
            DeviceToken = deviceToken
        }));
    }

    [HttpPut("customer/account/update-to-shop")]
    [Authorize(Roles = $"{IdentityConst.CustomerClaimName}")]
    public async  Task<IActionResult> UpdateCustomerToShop([FromForm] UpdateAccountToShopOwnerCommand updateAccountToShopOwner)
    {
        return HandleResult(await Mediator.Send(updateAccountToShopOwner));
    }

    [HttpGet("shop/login")]
    [Authorize(Roles = $"{IdentityConst.ShopClaimName}")]
    public async  Task<IActionResult> LoginToShop()
    {
        return HandleResult(await Mediator.Send(new LoginToShopQuery()));
    }

    [HttpGet("auth")]
    [Authorize]
    public async Task<IActionResult> CheckToken()
    {
        return this.HandleResult(await this.Mediator.Send(new VerifyTokenCommand()));
    }

    [HttpGet("auth/admin")]
    [Authorize(Roles = $"{IdentityConst.AdminClaimName}")]
    public async Task<IActionResult> CheckTokenWithRoleAdmin()
    {
        return Ok();
    }
}