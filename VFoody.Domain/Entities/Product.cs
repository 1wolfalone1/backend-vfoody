using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("product")]
[Index("ShopId", Name = "product_shop_FK")]
public partial class Product : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(200)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(400)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Description { get; set; } = null!;

    [Column("price")]
    public float Price { get; set; }

    [Column("image_url")]
    [StringLength(300)]
    public string ImageUrl { get; set; } = null!;

    [Column("total_order")]
    public int TotalOrder { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("shop_id")]
    public int ShopId { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    [InverseProperty("Product")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [ForeignKey("ShopId")]
    [InverseProperty("Products")]
    public virtual Shop Shop { get; set; } = null!;
}
