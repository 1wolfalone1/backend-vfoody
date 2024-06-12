namespace VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UpdateProfile;

public class UpdateProfileRequest
{
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    
    public float Latitude { get; set; }

    public float Longitude { get; set; }
}