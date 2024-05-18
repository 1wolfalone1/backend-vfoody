using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("account")]
[Index("BuildingId", Name = "account_building_FK")]
[Index("Email", Name = "account_email_unique", IsUnique = true)]
[Index("PhoneNumber", Name = "account_phone_number_unique", IsUnique = true)]
[Index("RoleId", Name = "account_role_FK")]
public partial class Account : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("phone_number")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Column("password")]
    [StringLength(250)]
    public string Password { get; set; } = null!;

    [Column("avatar_url")]
    [StringLength(300)]
    public string? AvatarUrl { get; set; }

    [Column("first_name")]
    [StringLength(150)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(150)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string LastName { get; set; } = null!;

    [Column("email")]
    [StringLength(200)]
    public string Email { get; set; } = null!;

    [Column("status")]
    public int Status { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("building_id")]
    public int? BuildingId { get; set; }
    
    [Column("refresh_token")]
    [StringLength(250)]
    public string? RefreshToken { get; set; }
    
    [Column("account_type")]
    public int AccountType { get; set; }

    [ForeignKey("BuildingId")]
    [InverseProperty("Accounts")]
    public virtual Building? Building { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();

    [InverseProperty("Account")]
    public virtual ICollection<FavouriteShop> FavouriteShops { get; set; } = new List<FavouriteShop>();

    [InverseProperty("Account")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Account")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("Account")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Account")]
    public virtual ICollection<PersonPromotion> PersonPromotions { get; set; } = new List<PersonPromotion>();

    [ForeignKey("RoleId")]
    [InverseProperty("Accounts")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("Account")]
    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
    
    [InverseProperty("Account")]
    public virtual ICollection<VerificationCode> VerificationCodes { get; set; } = new List<VerificationCode>();
}
