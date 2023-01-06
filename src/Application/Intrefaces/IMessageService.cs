using Domain.Entities;

namespace Application.Intrefaces
{
    public interface IMessageService
    {
        Message GetMessage(int userId);
        IEnumerable<Message> GetMessages(int userId, int messageCount);
        void AddMessage(RawMessage rawMessage);
        //Should return Dictionary with key: recipient id, value: number of messages from recipient
        Dictionary<int, int> AddMessages(IEnumerable<RawMessage> rawMessages);
    }
}
