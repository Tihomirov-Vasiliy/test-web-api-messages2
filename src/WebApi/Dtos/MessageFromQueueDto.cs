using Domain.Entities;

namespace WebApi.Dtos
{
    public class MessageFromQueueDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public MessageFromQueueDto(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
        public MessageFromQueueDto(Message message)
        {
            Subject = message.Subject;
            Body = message.Body;
        }
    }
}
