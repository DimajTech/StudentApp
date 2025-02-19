using static System.Runtime.InteropServices.JavaScript.JSType;
using StudentApp.Models.Entity;

namespace StudentApp.Models.DTO
{
    public class AppointmentDTO
    {
        public string? Id { get; set; }
        public DateTime Date { get; set; }
        public string Mode { get; set; }
        public string? Status { get; set; }
        public string CourseId { get; set; }
        public string StudentId { get; set; }
    }
}
