namespace StudentApp.Models.Entity
{
    public class ResponseAdvisement
    {
        private Guid id;
        //references to an object or just the id?
        private string advisementId; //references to other DB
        private string userId; //references to professor or student
        private string userRole; //student or professor
        private string text;
        private DateTime date;

        public ResponseAdvisement()
        {
        }
        
        //todo constructor
        public Guid Id { get => id; set => id = value; }
        public string AdvisementId { get => advisementId; set => advisementId = value; }
        public string UserId { get => userId; set => userId = value; }
        public string UserRole { get => userRole; set => userRole = value; }
        public string Text { get => text; set => text = value; }
        public DateTime Date { get => date; set => date = value; }

        



    }
}
