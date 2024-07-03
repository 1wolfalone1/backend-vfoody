namespace VFoody.Application.UseCases.Accounts.Models;

public class VerifyTokenResponse
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public string FullName { get; set; }
}