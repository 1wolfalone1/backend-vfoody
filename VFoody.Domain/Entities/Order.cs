using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("order")]
[Index("AccountId", Name = "order_account_FK")]
[Index("PersonalPromotionId", Name = "order_person_promotion_FK")]
[Index("PlatformPromotionId", Name = "order_platform_promotion_FK")]
[Index("ShopId", Name = "order_shop_FK")]
[Index("ShopPromotionId", Name = "order_shop_promotion_FK")]
[Index("TransactionId", Name = "order_transaction_FK")]
public partial class Order : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

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

    [ForeignKey("AccountId")]
    [InverseProperty("Orders")]
    public virtual Account Account { get; set; } = null!;
    
    [ForeignKey("BuildingId")]
    [InverseProperty("Orders")]
    public virtual Building Building { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("PersonalPromotionId")]
    [InverseProperty("Orders")]
    public virtual PersonPromotion? PersonalPromotion { get; set; }

    [ForeignKey("PlatformPromotionId")]
    [InverseProperty("Orders")]
    public virtual PlatformPromotion? PlatformPromotion { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("Orders")]
    public virtual Shop Shop { get; set; } = null!;

    [ForeignKey("ShopPromotionId")]
    [InverseProperty("Orders")]
    public virtual ShopPromotion? ShopPromotion { get; set; }

    [ForeignKey("TransactionId")]
    [InverseProperty("Orders")]
    public virtual Transaction Transaction { get; set; } = null!;
}
