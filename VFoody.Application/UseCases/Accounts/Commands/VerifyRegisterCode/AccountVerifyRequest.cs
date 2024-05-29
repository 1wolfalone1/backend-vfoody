namespace VFoody.Application.UseCases.Accounts.Commands.Verify;

public sealed record AccountVerifyRequest(string Email, int Code);