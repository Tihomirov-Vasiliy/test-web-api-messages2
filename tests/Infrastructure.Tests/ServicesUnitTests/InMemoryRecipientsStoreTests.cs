using Domain.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Stores;

namespace Infrastructure.Tests.ServicesUnitTests
{
    public class InMemoryRecipientsStoreTests
    {
        private InMemoryRecipientsStore _store = new InMemoryRecipientsStore();
        [Fact]
        public void AddMessageToQueue_RecipientExistInStore_AddMessageToMessageQueueOfExistingRecipient()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            RawMessage rawMessage = new RawMessage()
            {
                Subject = "0",
                Body = "0",
                RecipientIds = new List<int>()
                {
                    1
                }
            };
            _store.Recipients.Add(recipient);

            //Act
            _store.AddMessageToQueue(rawMessage);

            //Assert
            Assert.Equal(new Message(rawMessage.Subject, rawMessage.Body), _store.Recipients[0].Messages.Peek());
        }
        [Fact]
        public void AddMessageToQueue_RecipientNotExistInStore_AddNewRecipientInRecipients()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            RawMessage rawMessage = new RawMessage()
            {
                Subject = "0",
                Body = "0",
                RecipientIds = new List<int>()
                {
                    1
                }
            };

            //Act
            _store.AddMessageToQueue(rawMessage);

            //Assert
            Assert.Single(_store.Recipients);
        }
        [Fact]
        public void AddMessageToQueue_RecipientNotExistInStore_AddMessageToMessageQueueOfNewRecipient()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            RawMessage rawMessage = new RawMessage()
            {
                Subject = "0",
                Body = "0",
                RecipientIds = new List<int>()
                {
                    1
                }
            };

            //Act
            _store.AddMessageToQueue(rawMessage);

            //Assert
            Assert.Equal(new Message(rawMessage.Subject, rawMessage.Body), _store.Recipients[0].Messages.Peek());
        }
        [Fact]
        public void GetMessageByRecipientId_RecipientWithIdAndWithMessageExists_ReturnsMessage()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            Message actualMessage = new Message("subject", "body");
            recipient.Messages.Enqueue(actualMessage);
            _store.Recipients.Add(recipient);

            //Act + Assert
            Assert.Equal(_store.GetMessageByRecipientId(recipient.Id), actualMessage);
        }
        [Fact]
        public void GetMessageByRecipientId_RecipientWithIdAndWithoutMessageExists_ThrowsMessageNotFoundException()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            _store.Recipients.Add(recipient);

            //Act + Assert
            Assert.Throws(typeof(MessageNotFoundException), () => _store.GetMessageByRecipientId(recipient.Id));
        }
        [Fact]
        public void GetMessageByRecipientId_RecipientWithIdAndWithoutMessageExists_RemovesRecipientFromRecipientList()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            _store.Recipients.Add(recipient);

            //Act
            try
            {
                _store.GetMessageByRecipientId(recipient.Id);
            }
            catch (Exception)
            {

            }
            //Assert
            Assert.Empty(_store.Recipients);
        }
        [Fact]
        public void GetMessageByRecipientId_RecipientNotExists_ThrowMessageNotFoundException()
        {
            //Act + Assert
            Assert.Throws(typeof(MessageNotFoundException), () => _store.GetMessageByRecipientId(1));
        }
        [Fact]
        public void GetMessagesByRecipientIdAndCount_MessageCountEqualsMessageNumbersInList_ReturnsSameNumbersOfMessages()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            Message msg1 = new Message();
            Message msg2 = new Message();
            List<Message> messages = new List<Message>
            {
                msg1, msg2
            };
            recipient.Messages.Enqueue(msg1);
            recipient.Messages.Enqueue(msg2);
            _store.Recipients.Add(recipient);
            //Act + Assert
            Assert.Equal(messages, _store.GetMessagesByRecipientIdAndCount(1, 2));
        }
        [Theory]
        [InlineData(4)]
        [InlineData(1000)]
        public void GetMessagesByRecipientIdAndCount_MessageCountMoreMessageNumbersInList_ReturnsAllExistingMessages(int messageCount)
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            Message msg1 = new Message();
            Message msg2 = new Message();
            List<Message> messages = new List<Message>
            {
                msg1, msg2
            };
            recipient.Messages.Enqueue(msg1);
            recipient.Messages.Enqueue(msg2);
            _store.Recipients.Add(recipient);
            //Act + Assert
            Assert.Equal(messages, _store.GetMessagesByRecipientIdAndCount(1, messageCount));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(-100000)]
        public void GetMessagesByRecipientIdAndCount_WrongMessageCountValues_ThrowWrongMessageCountException(int messageCount)
        {
            //Act + Assert
            Assert.Throws(typeof(WrongMessageCountException), () => _store.GetMessagesByRecipientIdAndCount(1, messageCount));
        }
        [Fact]
        public void GetMessagesByRecipientIdAndCount_RecipientWithIdAndWithoutMessageExists_ThrowsMessageNotFoundException()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            _store.Recipients.Add(recipient);

            //Act + Assert
            Assert.Throws(typeof(MessageNotFoundException), () => _store.GetMessagesByRecipientIdAndCount(recipient.Id, 1));
        }
        [Fact]
        public void GetMessagesByRecipientIdAndCount_RecipientWithIdAndWithoutMessageExists_RemovesRecipientFromRecipientList()
        {
            //Arrange
            Recipient recipient = new Recipient(1);
            _store.Recipients.Add(recipient);

            //Act
            try
            {
                _store.GetMessagesByRecipientIdAndCount(recipient.Id, 1);
            }
            catch (Exception)
            {

            }
            //Assert
            Assert.Empty(_store.Recipients);
        }
        [Fact]
        public void GetMessagesByRecipientIdAndCount_RecipientNotExists_ThrowMessageNotFoundException()
        {
            //Act + Assert
            Assert.Throws(typeof(MessageNotFoundException), () => _store.GetMessagesByRecipientIdAndCount(1, 1));
        }
    }
}
