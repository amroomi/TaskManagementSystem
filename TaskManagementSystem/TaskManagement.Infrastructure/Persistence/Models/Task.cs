using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Infrastructure.Persistence.Models;

public partial class Task
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public int StatusId { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Tasks")]
    public virtual TaskStatus Status { get; set; } = null!;

    [InverseProperty("Task")]
    public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
}
