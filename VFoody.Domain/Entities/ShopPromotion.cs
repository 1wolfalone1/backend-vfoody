using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("shop_promotion")]
[Index("ShopId", Name = "shop_promotion_shop_FK")]
public partial class ShopPromotion : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

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

    [Column("start_date", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("usage_limit")]
    public int UsageLimit { get; set; }

    [Column("used")]
    public int Used { get; set; }

    [Column("shop_id")]
    public int ShopId { get; set; }

    [InverseProperty("ShopPromotion")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("ShopId")]
    [InverseProperty("ShopPromotions")]
    public virtual Shop Shop { get; set; } = null!;
}
