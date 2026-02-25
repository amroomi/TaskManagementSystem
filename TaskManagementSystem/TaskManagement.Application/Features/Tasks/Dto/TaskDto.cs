namespace TaskManagement.Application.Features.Tasks.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
    }
}