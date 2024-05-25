using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("platform_promotion")]
public partial class PlatformPromotion : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("banner_url")]
    [StringLength(300)]
    public string? BannerUrl { get; set; }

    [Column("amount_rate")]
    public float AmountRate { get; set; }

    [Column("minimum_order_value")]
    public float MinimumOrderValue { get; set; }

    [Column("maximum_apply_value")]
    public float MaximumApplyValue { get; set; }

    [Column("amount_value")]
    public float AmountValue { get; set; }

    [Column("apply_type")]
    public int ApplyType { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("usage_limit")]
    public int UsageLimit { get; set; }

    [Column("number_of_used")]
    public int NumberOfUsed { get; set; }

    [InverseProperty("PlatformPromotion")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
