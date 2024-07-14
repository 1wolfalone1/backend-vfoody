using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("shop_balance_history")]
[Index("ShopId", Name = "shop_balance_history_shop_FK")]
public partial class ShopBalanceHistory : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shop_id")]
    public int ShopId { get; set; }

    [Column("change_amount")]
    public float ChangeAmount { get; set; }

    [Column("balance_before_change")]
    public float BalanceBeforeChange { get; set; }

    [Column("balance_after_change")]
    public float BalanceAfterChange { get; set; }

    [Column("transaction_type")]
    public int TransactionType { get; set; }

    [Column("description")]
    [StringLength(400)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string? Description { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("ShopBalanceHistories")]
    public virtual Shop Shop { get; set; } = null!;
}
