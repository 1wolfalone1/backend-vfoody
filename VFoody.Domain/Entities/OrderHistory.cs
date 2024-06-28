using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VFoody.Domain.Entities;

namespace VFoody.Domain.Entities;

[Table("order_history")]
public partial class OrderHistory : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("reason")]
    [StringLength(512)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string? Reason { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("shipping_fee")]
    public float ShippingFee { get; set; }

    [Column("note")]
    [StringLength(300)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string? Note { get; set; }

    [Column("total_price")]
    public float TotalPrice { get; set; }

    [Column("total_promotion")]
    public float TotalPromotion { get; set; }

    [Column("full_name")]
    [StringLength(256)]
    public string FullName { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(100)]
    public string PhoneNumber { get; set; } = null!;

    [Column("is_refund", TypeName = "bit(1)")]
    public ulong IsRefund { get; set; }

    [Column("refund_status")]
    public int RefundStatus { get; set; }

    [Column("distance")]
    public float Distance { get; set; }

    [Column("duration_shipping", TypeName = "datetime")]
    public DateTime DurationShipping { get; set; }

    [Column("charge_fee")]
    public float ChargeFee { get; set; }

    [Column("transaction_id")]
    public int TransactionId { get; set; }

    [Column("shop_promotion_id")]
    public int? ShopPromotionId { get; set; }

    [Column("platform_promotion_id")]
    public int? PlatformPromotionId { get; set; }

    [Column("personal_promotion_id")]
    public int? PersonalPromotionId { get; set; }

    [Column("shop_id")]
    public int ShopId { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }

    [Column("building_id")]
    public int BuildingId { get; set; }
}
