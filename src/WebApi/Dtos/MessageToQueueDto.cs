using Domain;

namespace WebApi.Dtos
{
    public class MessageToQueueDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public ICollection<int> Recipients { get; set; }

        public Message MapToMessage()
        {
            return new Message
            {
                Subject = Subject,
                Body = Body,
                Recipients = Recipients
            };
        }
    }
}