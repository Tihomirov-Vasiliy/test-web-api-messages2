using Application.Intrefaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/messages")]
    public class MessagesController : ControllerBase
    {
        private IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet(Name = nameof(GetMessageByRecipientId))]
        public ActionResult<MessageFromQueueDto> GetMessageByRecipientId([Required][FromQuery] int rcpt)
        {
            return new MessageFromQueueDto(_messageService.GetMessage(rcpt));
        }

        [HttpPost(Name = nameof(AddMessageToQueue))]
        public ActionResult AddMessageToQueue([FromBody] MessageToQueueDto createMessageDto)
        {
            _messageService.AddMessage(createMessageDto.MapToRawMessage());

            return CreatedAtRoute(
                nameof(GetMessageByRecipientId),
                new { Id = createMessageDto.Recipients.FirstOrDefault() },
                new { RecipientIds = createMessageDto.Recipients });
        }

        [HttpGet("collections", Name = nameof(GetMessagesByRecipientId))]
        public ActionResult<IEnumerable<MessageFromQueueDto>> GetMessagesByRecipientId([Required][FromQuery] int rcpt, int messageCount = 1)
        {
            IEnumerable<Message> messages = _messageService.GetMessages(rcpt, messageCount);

            List<MessageFromQueueDto> results = new List<MessageFromQueueDto>();
            foreach (var message in messages)
                results.Add(new MessageFromQueueDto(message));

            return results;
        }

        [HttpPost("collections", Name = nameof(AddMessagesToQueue))]
        public ActionResult AddMessagesToQueue([FromBody] IEnumerable<MessageToQueueDto> createMessageDtos)
        {
            List<RawMessage> messages = new List<RawMessage>();
            foreach (var createMessageDto in createMessageDtos)
                messages.Add(createMessageDto.MapToRawMessage());

            _messageService.AddMessages(messages);

            //Creating List<int> to return ids in body for getting messages with user Ids and number of messages
            Dictionary<int, int> idToNumberOfMessages = new Dictionary<int, int>();
            foreach (var message in createMessageDtos)
                foreach (var recepientId in message.Recipients)
                {
                    if (!idToNumberOfMessages.ContainsKey(recepientId))
                        idToNumberOfMessages.Add(recepientId, 1);
                    else
                        idToNumberOfMessages[recepientId]++;
                }

            return CreatedAtRoute(
                nameof(GetMessageByRecipientId),
                new { rcpt = createMessageDtos.First().Recipients.FirstOrDefault() },
                new { RecepientIdsWithNumberOfMessages = idToNumberOfMessages.OrderBy(d => d.Key) });
        }
    }
}