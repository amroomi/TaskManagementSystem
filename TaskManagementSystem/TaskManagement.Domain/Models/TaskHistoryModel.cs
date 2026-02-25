using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Models
{
    public class TaskHistoryModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; } = null!;
        public DateTime ActionDate { get; set; }
    }
}