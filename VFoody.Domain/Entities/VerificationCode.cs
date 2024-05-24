using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("verification_code")]
[Index("AccountId", Name = "verification_code_account_FK")]
public partial class VerificationCode : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    [StringLength(100)]
    public string Code { get; set; } = null!;

    [Column("expired_tịme", TypeName = "datetime")]
    public DateTime ExpiredTịme { get; set; }

    [Column("code_type")]
    public int CodeType { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("VerificationCodes")]
    public virtual Account Account { get; set; } = null!;
}
