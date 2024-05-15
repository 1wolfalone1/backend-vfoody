using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("order_detail")]
[Index("OrderId", Name = "order_detail_order_FK")]
[Index("ProductId", Name = "order_detail_product_FK")]
public partial class OrderDetail : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price")]
    public float Price { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [InverseProperty("OrderDetail")]
    public virtual ICollection<OrderDetailOption> OrderDetailOptions { get; set; } = new List<OrderDetailOption>();

    [ForeignKey("ProductId")]
    [InverseProperty("OrderDetails")]
    public virtual Product Product { get; set; } = null!;
}
