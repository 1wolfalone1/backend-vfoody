﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VFoody.API.Identity;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;
using VFoody.Application.UseCases.Accounts.Commands.ForgotPasswordFirebase;
using VFoody.Application.UseCases.Accounts.Commands.Register;
using VFoody.Application.UseCases.Accounts.Commands.RegisterVerifyFirebase;
using VFoody.Application.UseCases.Accounts.Commands.SendCode;
using VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;
using VFoody.Application.UseCases.Accounts.Commands.VerifyRegisterCode;
using VFoody.Application.UseCases.Accounts.Queries;
using VFoody.Application.UseCases.Firebases.Commands.VerifyIdTokens;

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
    public async Task<IActionResult> VerifyIdToken([FromBody] string idToken)
    {
        return this.HandleResult(await this.Mediator.Send(new VerifyIDTokensCommand()
        {
            IdToken = idToken
        }));
    }
    
    [HttpPost("customer/firebase/forgot-password")]
    public async  Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordFirebaseRequest forgotPasswordFirebaseRequest)
    {
        return this.HandleResult(await this.Mediator.Send(new ForgotPasswordFirebaseCommand()
        {
            ForgotPasswordFirebaseRequest = forgotPasswordFirebaseRequest
        }));
    }
}