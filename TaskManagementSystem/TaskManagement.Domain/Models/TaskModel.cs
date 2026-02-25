namespace TaskManagement.Domain.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public int UserId { get; set; }
    }
}