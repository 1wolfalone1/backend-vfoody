using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("notification")]
[Index("AccountId", Name = "notification_account_FK")]
[Index("RoleId", Name = "notification_role_FK")]
public partial class Notification : BaseEntity
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

    [Column("readed", TypeName = "bit(1)")]
    public ulong Readed { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Notifications")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("Notifications")]
    public virtual Role Role { get; set; } = null!;
}
