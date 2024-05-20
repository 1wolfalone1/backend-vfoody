namespace VFoody.Application.UseCases.Accounts.Commands;

public sealed record CustomerRegisterRequest(string FirstName, string LastName, string Email, string Password);