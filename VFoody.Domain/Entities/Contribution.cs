using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("contribution")]
[Index("AccountId", Name = "contribution_account_FK")]
[Index("ContributionTypeId", Name = "contribution_contribution_type_FK")]
public partial class Contribution : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(200)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Title { get; set; } = null!;

    [Column("content")]
    [StringLength(400)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Content { get; set; } = null!;

    [Column("contribution_type_id")]
    public int ContributionTypeId { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Contributions")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("ContributionTypeId")]
    [InverseProperty("Contributions")]
    public virtual ContributionType ContributionType { get; set; } = null!;
}
