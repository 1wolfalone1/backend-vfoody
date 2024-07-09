namespace VFoody.Application.UseCases.Accounts.Models;

public class AccountInfoResponse
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string PhoneNumber { get; set; }
    public string RoleName { get; set; }

    public string Email { get; set; }

    public string AvatarUrl { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }

    public BuildingResponse Building { get; set; }
}