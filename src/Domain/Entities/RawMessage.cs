namespace Domain.Entities
{
    public class RawMessage
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<int> RecipientIds { get; set; } = new List<int>();
    }
}
