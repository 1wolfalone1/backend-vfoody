using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("person_promotion")]
[Index("AccountId", Name = "person_promotion_account_FK")]
public partial class PersonPromotion : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(300)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string? Title { get; set; }

    [Column("amount_rate")]
    public float AmountRate { get; set; }

    [Column("minimum_order_value")]
    public float MinimumOrderValue { get; set; }

    [Column("maximum_apply_value")]
    public float MaximumApplyValue { get; set; }

    [Column("amount_value")]
    public float AmountValue { get; set; }

    [Column("apply_type")]
    public int ApplyType { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("usage_limit")]
    public int UsageLimit { get; set; }

    [Column("number_of_used")]
    public int NumberOfUsed { get; set; }

    [Column("account_id")]
    public int AccountId { get; set; }
    
    [Column("description")]
    [StringLength(512)]
    public string? Description { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("PersonPromotions")]
    public virtual Account Account { get; set; } = null!;

    [InverseProperty("PersonalPromotion")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
