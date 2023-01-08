namespace Domain.Entities
{
    public class Recipient
    {
        public int Id { get; set; }
        public Queue<Message> Messages { get; set; } = new Queue<Message>();
        public Recipient(int id)
        {
            Id = id;
        }
    }
}
