namespace TaskManagement.Application.Features.TaskStatuses.Dto
{
    public class TaskStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}