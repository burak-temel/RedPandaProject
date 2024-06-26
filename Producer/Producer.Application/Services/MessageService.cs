using Common.Models;
using Confluent.Kafka;
using MongoDB.Driver;
using Producer.Application.Interfaces;
using Producer.Application.NewFolder;
using Producer.Domain.Models;
using Producer.Infrastructure.Interfaces;
using Producer.Services;

namespace Producer.Application.Services
{
    //public class MessageService : IMessageService
    //{
    //    private readonly IProducer<Null, string> _producer;
    //    private readonly IMessageRepository _messageRepository;
    //    private readonly string _topic = "test-topic";

    //    public MessageService(IProducer<Null, string> producer, IMessageRepository messageRepository)
    //    {
    //        _producer = producer;
    //        _messageRepository = messageRepository;
    //    }

    //    public async Task SendMessageAsync(string message)
    //    {
    //        // Send message to Red Panda (Kafka)
    //        var deliveryResult = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });

    //        // Save message to MongoDB
    //        var model = new ProducerModel { Name = "Example", Description = message };
    //        await _messageRepository.InsertAsync(model);
    //    }
    //}
    public class MessageService : IMessageService
    {
        private readonly IRedPandaService<MessageDTO, string> _redPandaService;
        private readonly IMessageRepository _messageRepository;
        private readonly string _topic = "test-topic";

        public MessageService(IRedPandaService<MessageDTO, string> redPandaService, IMessageRepository messageRepository)
        {
            _redPandaService = redPandaService;
            _messageRepository = messageRepository;
        }

      public async Task SendMessageAsync(ProducerDTO model)
        {
            var messageDTO = new MessageDTO { Name = model.Name, Description = model.Description };
            await _redPandaService.PublishRequestAsync(messageDTO, _topic);

            var mongoModel = new ProducerModel { Name = model.Name, Description = model.Description };
            // Save message to MongoDB
            await _messageRepository.InsertAsync(mongoModel);
        }
    }
}
