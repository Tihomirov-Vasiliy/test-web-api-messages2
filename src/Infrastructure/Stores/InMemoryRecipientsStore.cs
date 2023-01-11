using Domain.Entities;
using Infrastructure.Exceptions;

namespace Infrastructure.Stores
{
    public class InMemoryRecipientsStore
    {
        private List<Recipient> _recipients = new List<Recipient>();

        public void AddMessageToQueue(RawMessage rawMessage)
        {
            Recipient recipient;

            foreach (int recipientId in rawMessage.RecipientIds)
            {
                if ((recipient = GetRecipientById(recipientId)) == null)
                {
                    recipient = new Recipient(recipientId);
                    _recipients.Add(recipient);
                }
                recipient.Messages.Enqueue(new Message(rawMessage.Subject, rawMessage.Body));
            }
        }

        public Message GetMessageByRecipientId(int recipientId)
        {
            Recipient recipient;
            if ((recipient = GetRecipientById(recipientId)) == null)
                throw new MessageNotFoundException($"Message for recipient with id:{recipientId} not found");

            Message message = recipient.Messages.Dequeue();

            if (recipient.Messages.Count == 0)
                _recipients.Remove(recipient);

            return message;
        }

        public IEnumerable<Message> GetMessagesByRecipientIdAndCount(int recipientId, int messageCount)
        {
            if (messageCount <= 0)
                throw new WrongMessageCountException($"Message Count value should be >= 1, actual value:{messageCount}");

            List<Message> messages = new List<Message>();
            Recipient recipient;

            if ((recipient = GetRecipientById(recipientId)) == null)
                throw new MessageNotFoundException($"Message for recipient with id:{recipientId} not found");

            for (int i = 0; i < messageCount; i++)
            {
                Message message = recipient.Messages.Dequeue();
                messages.Add(message);
                if (recipient.Messages.Count == 0)
                {
                    _recipients.Remove(recipient);
                    return messages;
                }
            }
            return messages;
        }

        private Recipient GetRecipientById(int recipientId)
        {
            return _recipients.FirstOrDefault(r => r.Id == recipientId);
        }
    }
}
