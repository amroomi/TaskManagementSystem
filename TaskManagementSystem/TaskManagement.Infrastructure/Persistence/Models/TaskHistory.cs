using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Infrastructure.Persistence.Models;

[Table("TaskHistory")]
public partial class TaskHistory
{
    [Key]
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    [StringLength(50)]
    public string Action { get; set; } = null!;

    public DateTime ActionDate { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskHistories")]
    public virtual Task Task { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TaskHistories")]
    public virtual User User { get; set; } = null!;
}
