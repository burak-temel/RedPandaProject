using Confluent.Kafka;
using MongoDB.Driver;
using Producer.Application.Interfaces;
using Producer.Domain.Models;
using Producer.Infrastructure.Interfaces;

namespace Producer.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IMessageRepository _messageRepository;
        private readonly string _topic = \
test-topic\;

        public MessageService(IProducer<Null, string> producer, IMessageRepository messageRepository)
        {
            _producer = producer;
            _messageRepository = messageRepository;
        }

        public async Task SendMessageAsync(string message)
        {
            // Send message to Red Panda (Kafka)
            var deliveryResult = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });

            // Save message to MongoDB
            var model = new AbcModel { Name = \Example\, Description = message };
            await _messageRepository.InsertAsync(model);
        }
    }
}
