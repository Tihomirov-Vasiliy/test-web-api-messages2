using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class MessageToQueueDto
    {
        [Required]
        public string Subject { get; set; } 

        [Required]
        public string Body { get; set; }

        [Required]
        public IList<int> Recipients { get; set; }

        public RawMessage MapToRawMessage()
        {
            return new RawMessage { Subject = Subject, Body = Body, RecipientIds = Recipients };
        }
    }
}