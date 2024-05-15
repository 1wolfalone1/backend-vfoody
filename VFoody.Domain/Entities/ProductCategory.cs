using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[PrimaryKey("CategoryId", "ProductId")]
[Table("product_category")]
[Index("ProductId", Name = "product_category_product_FK")]
public partial class ProductCategory : BaseEntity
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("ProductCategories")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("ProductCategories")]
    public virtual Product Product { get; set; } = null!;
}
