namespace VFoody.Application.UseCases.Accounts.Commands.SendCode;

public sealed record AccountSendCodeRequest(string Email, int VerifyType);