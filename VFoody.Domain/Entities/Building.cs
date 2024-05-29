using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("building")]
public partial class Building : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(200)]
    [MySqlCharSet("utf8mb3")]
    [MySqlCollation("utf8mb3_general_ci")]
    public string Name { get; set; } = null!;

    [InverseProperty("Building")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [InverseProperty("BuildingIdFromNavigation")]
    public virtual ICollection<Distance> DistanceBuildingIdFromNavigations { get; set; } = new List<Distance>();

    [InverseProperty("BuildingIdToNavigation")]
    public virtual ICollection<Distance> DistanceBuildingIdToNavigations { get; set; } = new List<Distance>();

    [InverseProperty("Building")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
    [InverseProperty("Building")]
    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
}
