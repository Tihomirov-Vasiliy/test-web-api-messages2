using Domain.Entities;

namespace Application.Intrefaces
{
    public interface IMessageService
    {
        Message GetMessage(int userId);
        IEnumerable<Message> GetMessages(int userId, int messageCount);
        void AddMessage(RawMessage rawMessage);
        void AddMessages(IEnumerable<RawMessage> rawMessages);
    }
}
