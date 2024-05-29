using Microsoft.AspNetCore.Mvc;
using Consumer.Application.Interfaces;
using System.Threading.Tasks;

namespace Consumer.API.Controllers
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
        public async Task<IActionResult> ReceiveMessage([FromBody] string message)
        {
            try
            {
                await _messageService.ProcessMessageAsync(message);
                return Ok(new { Status = "Success", Message = message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", ex.Message });
            }
        }
    }
}
