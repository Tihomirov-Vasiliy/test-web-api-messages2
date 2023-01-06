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

        public Dictionary<int, int> AddMessages(IEnumerable<RawMessage> rawMessages)
        {
            //Creating List<int> to return ids in body for getting messages with user Ids and number of messages
            Dictionary<int, int> idToNumberOfMessages = new Dictionary<int, int>();
            foreach (var rawMessage in rawMessages)
            {
                _memoryStore.AddMessageToQueue(rawMessage);
                foreach (var recepientId in rawMessage.RecipientIds)
                {
                    if (!idToNumberOfMessages.ContainsKey(recepientId))
                        idToNumberOfMessages.Add(recepientId, 1);
                    else
                        idToNumberOfMessages[recepientId]++;
                }
            }
            return idToNumberOfMessages;
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
