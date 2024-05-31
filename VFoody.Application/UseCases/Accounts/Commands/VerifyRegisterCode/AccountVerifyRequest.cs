namespace VFoody.Application.UseCases.Accounts.Commands.VerifyRegisterCode;

public sealed record AccountVerifyRequest(string Email, int Code);