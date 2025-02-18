using Microsoft.Identity.Client;

namespace StudentApp.Models.DTO
{
    public class UpdateStudentDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string LinkedIn { get; set; }
        public string Picture { get; set; }
        public string? ProfessionalBackground { get; set; }   
    }
}
