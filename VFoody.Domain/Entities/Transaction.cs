using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("transaction")]
public partial class Transaction : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("amount")]
    public float Amount { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("transaction_type")]
    public int TransactionType { get; set; }

    [Column("payment_thirdparty_id")]
    [StringLength(200)]
    public string? PaymentThirdpartyId { get; set; }

    [Column("payment_thirdparty_content")]
    [StringLength(200)]
    public string? PaymentThirdpartyContent { get; set; }

    [InverseProperty("Transaction")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
