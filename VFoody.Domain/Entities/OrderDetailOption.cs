using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[PrimaryKey("OrderDetailId", "OptionId")]
[Table("order_detail_option")]
[Index("OptionId", Name = "order_detail_option_option_fk")]
public partial class OrderDetailOption : BaseEntity
{
    [Key]
    [Column("order_detail_id")]
    public int OrderDetailId { get; set; }

    [Key]
    [Column("option_id")]
    public int OptionId { get; set; }

    [Column("price")]
    public float Price { get; set; }

    [ForeignKey("OptionId")]
    [InverseProperty("OrderDetailOptions")]
    public virtual Option Option { get; set; } = null!;

    [ForeignKey("OrderDetailId")]
    [InverseProperty("OrderDetailOptions")]
    public virtual OrderDetail OrderDetail { get; set; } = null!;
}
