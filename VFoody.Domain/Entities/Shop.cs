using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("shop")]
[Index("AccountId", Name = "shop_account_FK")]
[Index("BuildingId", Name = "shop_building_FK")]
[Index("PhoneNumber", Name = "shop_phone_number_unique", IsUnique = true)]
public partial class Shop : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(200)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Name { get; set; } = null!;

    [Column("logo_url")]
    [StringLength(300)]
    public string? LogoUrl { get; set; }

    [Column("banner_url")]
    [StringLength(300)]
    public string? BannerUrl { get; set; }

    [Column("description")]
    [StringLength(400)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string? Description { get; set; }

    [Column("balance")]
    public float Balance { get; set; }

    [Column("phone_number")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;
    
    [Column("active_from")]
    public int ActiveFrom { get; set; }

    [Column("active_to")]
    public int ActiveTo { get; set; }

    [Column("active", TypeName = "bit(1)")]
    public ulong Active { get; set; }

    [Column("total_order")]
    public int TotalOrder { get; set; }

    [Column("total_product")]
    public int TotalProduct { get; set; }

    [Column("total_rating")]
    public int TotalRating { get; set; }
    
    [Column("total_star")]
    public int TotalStar { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("minimum_value_order_freeship")]
    public float MinimumValueOrderFreeship { get; set; }

    [Column("shipping_fee")]
    public float ShippingFee { get; set; }

    [Column("building_id")]
    public int BuildingId { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Shops")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("BuildingId")]
    [InverseProperty("Shops")]
    public virtual Building Building { get; set; } = null!;

    [InverseProperty("Shop")]
    public virtual ICollection<FavouriteShop> FavouriteShops { get; set; } = new List<FavouriteShop>();

    [InverseProperty("Shop")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Shop")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("Shop")]
    public virtual ICollection<ShopPromotion> ShopPromotions { get; set; } = new List<ShopPromotion>();
    
    [InverseProperty("Shop")]
    public virtual ICollection<ShopBalanceHistory> ShopBalanceHistories { get; set; } = new List<ShopBalanceHistory>();
    
    [InverseProperty("Shop")]
    public virtual ICollection<ShopWithdrawalRequest> ShopWithdrawalRequests { get; set; } = new List<ShopWithdrawalRequest>();
}
