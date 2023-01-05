using Domain;

namespace Application.Intrefaces
{
    public interface IMessageService
    {
        Message GetMessageFromQueueById(int id);
        void AddMessageToQueue(Message message);
    }
}
