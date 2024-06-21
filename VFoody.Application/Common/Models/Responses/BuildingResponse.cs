namespace VFoody.Application.Common.Models.Responses;

public class BuildingResponse
{
    public int BuildingId { get; set; }
    public string Address { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
}