using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("commission_config")]
public partial class CommissionConfig : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("commission_rate")]
    public float CommissionRate { get; set; }
}
