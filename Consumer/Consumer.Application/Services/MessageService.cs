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

        public async Task ProcessMessageAsync(string message)
        {
            // Save message to MongoDB
            var model = new ConsumerModel { Name = "Example", Description = message };
            await _messageRepository.InsertAsync(model);
        }
    }
}
