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

        [HttpGet("batch", Name = nameof(GetMessagesByRecipientId))]
        public ActionResult<IEnumerable<MessageFromQueueDto>> GetMessagesByRecipientId([Required][FromQuery] int rcpt, int messageCount = 1)
        {
            IEnumerable<Message> messages = _messageService.GetMessages(rcpt, messageCount);

            List<MessageFromQueueDto> results = new List<MessageFromQueueDto>();
            foreach (var message in messages)
                results.Add(new MessageFromQueueDto(message));

            return results;
        }

        [HttpPost("batch", Name = nameof(AddMessagesToQueue))]
        public ActionResult AddMessagesToQueue([FromBody] IEnumerable<MessageToQueueDto> createMessageDtos)
        {
            List<RawMessage> messages = new List<RawMessage>();
            foreach (var createMessageDto in createMessageDtos)
                messages.Add(createMessageDto.MapToRawMessage());

            Dictionary<int, int> idToNumberOfMessages = _messageService.AddMessages(messages);

            return CreatedAtRoute(
                nameof(GetMessageByRecipientId),
                new { rcpt = createMessageDtos.First().Recipients.FirstOrDefault() },
                new { RecepientIdsWithNumberOfMessages = idToNumberOfMessages.OrderBy(d => d.Key) });
        }
    }
}