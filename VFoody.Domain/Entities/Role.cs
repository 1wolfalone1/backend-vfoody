using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VFoody.Domain.Entities;

[Table("role")]
public partial class Role : BaseEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [InverseProperty("Role")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
