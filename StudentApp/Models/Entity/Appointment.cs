using System.Runtime.CompilerServices;

namespace StudentApp.Models.Entity
{
    public class Appointment
    {
        private Guid id;
        private DateTime date;
        private string mode;
        private string status;
        private Course course;
        private User user;
        private string professorComment;

        public Appointment()
        {
        }

        public Appointment(string mode, Course course, User user)
        {
            id = Guid.NewGuid();
            this.date = DateTime.Now;
            this.mode = mode;
            this.status = "pending"; //VALOR DEFAULT
            this.course = course; 
            this.user = user;
        }

        public Appointment(DateTime date, string mode, string status, Course course) //para el AppointmentDAO
        {


        }

        public Guid Id { get => id; set => id = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Mode { get => mode; set => mode = value; }
        public string Status { get => status; set => status = value; }
        public Course Course { get => course; set => course = value; }
        public User User { get => user; set => user = value; }
        public string ProfessorComment { get => professorComment; set => professorComment = value; }
    }
}
