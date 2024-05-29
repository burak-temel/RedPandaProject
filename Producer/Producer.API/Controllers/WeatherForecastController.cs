using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using System.Threading.Tasks;

namespace Producer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic = "test-topic"; // Red Panda topic

        public MessagesController(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
                return Ok(new { Status = "Success", Message = message, Offset = deliveryResult.Offset });
            }
            catch (ProduceException<Null, string> ex)
            {
                return StatusCode(500, new { Status = "Error", ex.Message });
            }
        }
    }
}
