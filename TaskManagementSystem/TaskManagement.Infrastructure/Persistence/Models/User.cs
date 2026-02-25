using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Infrastructure.Persistence.Models;

[Index("Username", Name = "UQ__Users__536C85E4065E69E8", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(100)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
}
