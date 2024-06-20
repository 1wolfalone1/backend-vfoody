namespace VFoody.Application.UseCases.Accounts.Models;

public class ManageAccountResponse
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string PhoneNumber { get; set; }
    public string AvatarUrl { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
}