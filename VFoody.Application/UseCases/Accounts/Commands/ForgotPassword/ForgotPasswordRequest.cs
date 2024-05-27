namespace VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;

public sealed record ForgotPasswordRequest(string Email, int Code, string NewPassword);