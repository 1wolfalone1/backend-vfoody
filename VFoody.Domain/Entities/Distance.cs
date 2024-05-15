using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VFoody.Domain.Entities;

[Table("distance")]
[Index("BuildingIdFrom", Name = "distance_from_building_FK")]
[Index("BuildingIdTo", Name = "distance_to_building_FK")]
public partial class Distance : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("distance")]
    public float Distance1 { get; set; }

    [Column("building_id_from")]
    public int BuildingIdFrom { get; set; }

    [Column("building_id_to")]
    public int BuildingIdTo { get; set; }

    [ForeignKey("BuildingIdFrom")]
    [InverseProperty("DistanceBuildingIdFromNavigations")]
    public virtual Building BuildingIdFromNavigation { get; set; } = null!;

    [ForeignKey("BuildingIdTo")]
    [InverseProperty("DistanceBuildingIdToNavigations")]
    public virtual Building BuildingIdToNavigation { get; set; } = null!;
}
