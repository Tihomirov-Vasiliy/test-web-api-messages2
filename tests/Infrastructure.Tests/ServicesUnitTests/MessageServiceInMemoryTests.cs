using Domain.Entities;
using Infrastructure.Services;
using Infrastructure.Stores;

namespace Infrastructure.Tests.ServicesUnitTests
{
    public class MessageServiceInMemoryTests
    {
        private MessageServiceInMemory _messageServiceInMemory;
        public MessageServiceInMemoryTests()
        {
            _messageServiceInMemory = new MessageServiceInMemory(new InMemoryRecipientsStore());
        }
        [Fact]
        public void AddMessage_NullRawMessage_ThrowsArgumentNullException()
        {
            //Arrange
            RawMessage rawMessage = null;
            //Act+Assert
            Assert.Throws(typeof(ArgumentNullException), () => _messageServiceInMemory.AddMessage(rawMessage));
        }
        [Fact]
        public void AddMessages_NullRawMessages_ThrowsArgumentNullException()
        {
            //Arrange
            IEnumerable<RawMessage> messages = null;
            //Act+Assert
            Assert.Throws(typeof(ArgumentNullException), () => _messageServiceInMemory.AddMessages(messages));
        }
        [Fact]
        public void AddMessages_ZeroCountOfRawMessages_ThrowsArgumentNullException()
        {
            //Arrange
            IEnumerable<RawMessage> messages = new List<RawMessage>();
            //Act+Assert
            Assert.Throws(typeof(ArgumentNullException), () => _messageServiceInMemory.AddMessages(messages));
        }
        [Fact]
        public void AddMessages_RawMessagesWithRecipients_ReturnsDictionaryWithIdOfRecipientAndCountOfMessages()
        {
            //Arrange
            IEnumerable<RawMessage> messages = new List<RawMessage>()
            {
                new RawMessage()
                {
                     RecipientIds = new List<int>(){1,2,3 }
                },
                new RawMessage()
                {
                     RecipientIds = new List<int>(){1,2 }
                }
            };
            Dictionary<int, int> idToNumberOfMessages = new Dictionary<int, int>();
            idToNumberOfMessages.Add(1,2);
            idToNumberOfMessages.Add(2,2);
            idToNumberOfMessages.Add(3,1);

            //Act+Assert
            Assert.Equal(idToNumberOfMessages, _messageServiceInMemory.AddMessages(messages));
        }
    }
}
