namespace Domain.Entities
{
    public class Message
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public Message()
        {

        }
        public Message(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
        public override bool Equals(object? obj)
        {
            Message message = obj as Message;
            if (obj == null)
                return false;
            if (Subject == message.Subject && Body == message.Body)
                return true;
            return false;
        }
    }
}