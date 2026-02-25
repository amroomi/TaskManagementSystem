using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Infrastructure.Persistence.Models;

[Index("Name", Name = "UQ__TaskStat__737584F67799A7E5", IsUnique = true)]
public partial class TaskStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string? Description { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
