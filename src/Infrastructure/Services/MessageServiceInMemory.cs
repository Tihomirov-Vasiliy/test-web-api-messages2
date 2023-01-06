using Application.Intrefaces;
using Domain.Entities;
using Infrastructure.Stores;

namespace Infrastructure.Services
{
    public class MessageServiceInMemory : IMessageService
    {
        private InMemoryRecipientsStore _memoryStore;

        public MessageServiceInMemory(InMemoryRecipientsStore memoryStore)
        {
            _memoryStore = memoryStore;
        }

        public void AddMessage(RawMessage rawMessage)
        {
            _memoryStore.AddMessageToQueue(rawMessage);
        }

        public void AddMessages(IEnumerable<RawMessage> rawMessages)
        {
            foreach (var rawMessage in rawMessages)
            {
                _memoryStore.AddMessageToQueue(rawMessage);
            }
        }

        public Message GetMessage(int userId)
        {
            return _memoryStore.GetMessageByRecipientId(userId);
        }

        public IEnumerable<Message> GetMessages(int userId, int messageCount)
        {
            return _memoryStore.GetMessagesByRecipientIdAndCount(userId, messageCount);
        }
    }
}
