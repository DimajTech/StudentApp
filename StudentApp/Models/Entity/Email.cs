namespace StudentApp.Models.Entity
{
    public class Email
    {
        private string toUser;
        private string subject;
        private string content;

        public Email()
        {

        }
        public Email(string toUser, string subject, string content)
        {
            this.toUser = toUser;
            this.subject = subject;
            this.content = content;
        }

        public string ToUser { get => toUser; set => toUser = value; }
        public string Subject { get => subject; set => subject = value; }
        public string Content { get => content; set => content = value; }
    }
}
