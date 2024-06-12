namespace VFoody.Application.UseCases.Accounts.Models;

public class BuildingResponse
{
    public int Id { get; set; }
    public string Address { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}