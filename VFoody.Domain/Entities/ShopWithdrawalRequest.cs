using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VFoody.Domain.Entities;

namespace VFoody.Domain.Entities;

[Table("shop_withdrawal_requests")]
[Index("ProcessedBy", Name = "shop_withdrawal_requests_processed_by_FK")]
[Index("ShopId", Name = "shop_withdrawal_requests_shop_FK")]
public partial class ShopWithdrawalRequest : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shop_id")]
    public int ShopId { get; set; }

    [Column("requested_amount")]
    public float RequestedAmount { get; set; }

    [Column("status")]
    public int Status { get; set; }
    
    [Column("bank_code")]
    public int BankCode { get; set; }
    
    [Column("bank_short_name")]
    public string BankShortName { get; set; }
    
    [Column("bank_account_number")]
    public string BankAccountNumber { get; set; }
    
    [Column("note")]
    public string? Note { get; set; }

    [Column("requested_date", TypeName = "datetime")]
    public DateTime RequestedDate { get; set; }

    [Column("processed_date", TypeName = "datetime")]
    public DateTime? ProcessedDate { get; set; }

    [Column("processed_by")]
    public int? ProcessedBy { get; set; }

    [ForeignKey("ProcessedBy")]
    [InverseProperty("ShopWithdrawalRequests")]
    public virtual Account? ProcessedByNavigation { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("ShopWithdrawalRequests")]
    public virtual Shop Shop { get; set; } = null!;
}
