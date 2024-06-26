using Microsoft.AspNetCore.Mvc;
using Consumer.Application.Interfaces;
using System.Threading.Tasks;
using Consumer.Domain.Models;
using Consumer.Application.DTOs;

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
        public async Task<IActionResult> ReceiveMessage([FromBody] ConsumerDTO message)
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
