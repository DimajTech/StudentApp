using Microsoft.AspNetCore.Http.HttpResults;

namespace StudentApp.Models.DTO
{
    public class CreateAdvisementDTO
    {
        public string? Id { get; set; }
        public string CourseId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public bool IsPublic { get; set; }
        public string StudentId { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
