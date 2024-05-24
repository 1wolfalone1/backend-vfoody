using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("option")]
[Index("QuestionId", Name = "option_question_FK")]
public partial class Option : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("description")]
    [StringLength(300)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Description { get; set; } = null!;

    [Column("is_pricing", TypeName = "bit(1)")]
    public ulong IsPricing { get; set; }

    [Column("price")]
    public float Price { get; set; }
    
    [Column("image_url")]
    [StringLength(300)]
    public string? ImageUrl { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("question_id")]
    public int QuestionId { get; set; }

    [InverseProperty("Option")]
    public virtual ICollection<OrderDetailOption> OrderDetailOptions { get; set; } = new List<OrderDetailOption>();

    [ForeignKey("QuestionId")]
    [InverseProperty("Options")]
    public virtual Question Question { get; set; } = null!;
}
