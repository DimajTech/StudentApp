namespace StudentApp.Models.DTO
{
    public class CourseCrudDTO
    {
        public string? id { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public string? professorId { get; set; }
        public string? professorName { get; set; }
        public string? semester { get; set; }
        public int? year { get; set; }
        public bool? isActive { get; set; }
    }
}
