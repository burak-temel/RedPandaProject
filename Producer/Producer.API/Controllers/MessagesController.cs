using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using System.Threading.Tasks;
using Producer.Application.Interfaces;
using Producer.Domain.Models;
using Producer.Application.NewFolder;

namespace Producer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ProducerDTO message)
        {
            try
            {
                await _messageService.SendMessageAsync(message);
                return Ok(new
                {
                    Status = "Success",
                    Message = message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    ex.Message
                });
            }
        }

    }
}
