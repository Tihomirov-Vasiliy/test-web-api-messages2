namespace Domain
{
    public class Message
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public ICollection<int> Recipients { get; set; }
    }
}