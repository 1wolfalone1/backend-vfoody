namespace VFoody.Application.UseCases.Accounts.Commands.Register;

public sealed record CustomerRegisterRequest(string PhoneNumber, string Email, string Password);