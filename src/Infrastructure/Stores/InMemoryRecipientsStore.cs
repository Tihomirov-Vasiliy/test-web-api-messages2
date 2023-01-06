using Domain.Entities;
using Infrastructure.Exceptions;

namespace Infrastructure.Stores
{
    public class InMemoryRecipientsStore
    {
        public List<Recipient> Recipients { get; set; } = new List<Recipient>();

        public void AddMessageToQueue(RawMessage rawMessage)
        {
            Recipient recipient;

            foreach (var recipientId in rawMessage.RecipientIds)
            {
                if ((recipient = GetRecipientById(recipientId)) == null)
                {
                    recipient = new Recipient(recipientId);
                    Recipients.Add(recipient);
                }
                recipient.Messages.Enqueue(new Message(rawMessage.Subject, rawMessage.Body));
            }
        }

        public Message GetMessageByRecipientId(int recipientId)
        {
            Recipient recipient;
            if ((recipient = GetRecipientById(recipientId)) == null)
                throw new MessageNotFoundException($"Message for recipient with id:{recipientId} not found");

            if (recipient.Messages.TryDequeue(out Message recipienMessage))
                return recipienMessage;
            else
            {
                Recipients.Remove(recipient);
                throw new MessageNotFoundException($"Message for recipient with id:{recipientId} not found");
            }
        }

        public IEnumerable<Message> GetMessagesByRecipientIdAndCount(int recipientId, int messageCount)
        {
            List<Message> messages = new List<Message>();
            Recipient recipient;

            recipient = GetRecipientById(recipientId);

            if (recipient == null)
                throw new MessageNotFoundException($"Message for recipient with id:{recipientId} not found");

            for (int i = 0; i < messageCount; i++)
            {
                if (recipient.Messages.TryDequeue(out Message recipientMessage))
                    messages.Add(recipientMessage);
                else if (i == 0)
                {
                    Recipients.Remove(recipient);
                    throw new MessageNotFoundException($"Message for recipient with id:{recipientId} not found");
                }
                else
                    return messages;
            }
            return messages;
        }

        private Recipient GetRecipientById(int recipientId)
        {
            return Recipients
                .FirstOrDefault(r => r.Id == recipientId);
        }
    }
}
