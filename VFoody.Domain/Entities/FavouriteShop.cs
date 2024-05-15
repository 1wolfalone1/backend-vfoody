using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("favourite_shop")]
[Index("AccountId", Name = "favourite_shop_account_FK")]
[Index("ShopId", Name = "favourite_shop_shop_FK")]
public partial class FavouriteShop : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shop_id")]
    public int ShopId { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("FavouriteShops")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("ShopId")]
    [InverseProperty("FavouriteShops")]
    public virtual Shop Shop { get; set; } = null!;
}
