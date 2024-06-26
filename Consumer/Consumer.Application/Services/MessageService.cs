using Common.Models;
using Consumer.Application.DTOs;
using Consumer.Application.Interfaces;
using Consumer.Domain.Models;
using Consumer.Infrastructure.Interfaces;

namespace Consumer.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task ProcessMessageAsync(MessageDTO message)
        {
            // Save message to MongoDB
            var model = new ConsumerModel { Name = message.Name, Description = message.Description };
            await _messageRepository.SaveMessageAsync(model);
        }

        public async Task ProcessMessageAsync(ConsumerDTO message)
        {
            var mongoModel = new ConsumerModel { Name = message.Name, Description = message.Description };
            await _messageRepository.SaveMessageAsync(mongoModel);
        }
    }

}
