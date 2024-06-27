using System.Text.Json.Serialization;
using VFoody.Domain.Enums;

namespace VFoody.Application.UseCases.Promotion.Models;

public class AllPromotionResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string BannerUrl { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int ApplyType { get; set; }
    public decimal AmountRate { get; set; }
    public decimal AmountValue { get; set; }
    public decimal MinimumOrderValue { get; set; }
    public decimal MaximumApplyValue { get; set; }

    public int UsageLimit { get; set; }
    public int NumberOfUsed { get; set; }
    public int Status { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public PromotionTypes PromotionType { get; set; }

    [JsonIgnore]
    public int TotalItems { get; set; }
}