using System.Text.Json.Serialization;
using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.Feedbacks.Models;

public class CreateFeedbackResponse
{
    public int Id { get; set; }
    public string Comment { get; set; }
    public RatingRanges Rating { get; set; }
    public string[] Images
    {
        get => this.ImagesUrl.Split(",");
    }
    public DateTime CreatedDate { get; set; }
    
    [JsonIgnore]
    public string ImagesUrl { get; set; }
}