namespace StudentApp.Models.DTO
{
    public class InsertProfessorDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? RegistrationStatus { get; set; }
        public string Role { get; set; }
    }
}
