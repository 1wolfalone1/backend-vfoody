namespace VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;

public sealed record VerifyCodeForgotPasswordRequest(string Email, int Code);