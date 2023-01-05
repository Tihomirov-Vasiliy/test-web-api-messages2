using Application.Intrefaces;
using Domain;

namespace Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        public void AddMessageToQueue(Message message)
        {
            Console.WriteLine("AddMessageToQueue");
        }

        public Message GetMessageFromQueueById(int id)
        {
            Console.WriteLine($"GetMessageFromQueueById:{id}");
            return new Message() { Body = "TestBody", Subject = "TestSubject" };
        }
    }
}
