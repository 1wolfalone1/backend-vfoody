using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("question")]
[Index("ProductId", Name = "question_product_FK")]
public partial class Question : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("question_type")]
    public int QuestionType { get; set; }

    [Column("description")]
    [StringLength(300)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Description { get; set; } = null!;

    [Column("status")]
    public int Status { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<Option> Options { get; set; } = new List<Option>();

    [ForeignKey("ProductId")]
    [InverseProperty("Questions")]
    public virtual Product Product { get; set; } = null!;
}
