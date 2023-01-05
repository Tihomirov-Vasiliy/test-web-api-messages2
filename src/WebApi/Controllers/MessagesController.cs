using Application.Intrefaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/messages")]
    [ApiVersion("1.0")]
    public class MessagesController : ControllerBase
    {
        private IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpGet("{id}", Name = "GetMessageFromQueueById")]
        public ActionResult<MessageFromQueueDto> GetMessageFromQueueById(int id)
        {
            return new MessageFromQueueDto(_messageService.GetMessageFromQueueById(id));
        }

        [HttpPost]
        public ActionResult AddMessageToQueue([FromBody] MessageToQueueDto createMessage)
        {
            _messageService.AddMessageToQueue(createMessage.MapToMessage());

            return CreatedAtRoute(nameof(GetMessageFromQueueById), new { Id = createMessage.Recipients.FirstOrDefault() }, createMessage.Recipients);
        }

    }
}
