namespace TaskManagement.Domain.Models
{
    public class TaskStatusModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}