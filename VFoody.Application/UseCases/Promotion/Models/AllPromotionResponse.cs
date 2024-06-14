﻿using System.Text.Json.Serialization;

namespace VFoody.Application.UseCases.Promotion.Models;

public class AllPromotionResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
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
    
    public string PromotionName { get; set; }

    [JsonIgnore]
    public int TotalItems { get; set; }
}