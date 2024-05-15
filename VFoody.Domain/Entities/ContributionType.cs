using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("contribution_type")]
public partial class ContributionType : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(200)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Title { get; set; } = null!;

    [Column("description")]
    [StringLength(400)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Description { get; set; } = null!;

    [InverseProperty("ContributionType")]
    public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
}
