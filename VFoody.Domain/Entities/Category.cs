using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("category")]
public partial class Category : BaseEntity
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

    [InverseProperty("Category")]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
