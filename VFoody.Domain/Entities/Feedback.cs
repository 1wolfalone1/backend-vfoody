using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("feedback")]
[Index("AccountId", Name = "feedback_account_FK")]
[Index("OrderId", Name = "feedback_order_FK")]
public partial class Feedback : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("rating")]
    public int Rating { get; set; }

    [Column("comment")]
    [StringLength(300)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string? Comment { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Feedbacks")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("Feedbacks")]
    public virtual Order Order { get; set; } = null!;
}
