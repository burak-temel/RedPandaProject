using System.Threading.Tasks;
using Moq;
using Xunit;
using Consumer.Application.Interfaces;
using Consumer.Application.Services;
using Consumer.Infrastructure.Interfaces;
using Consumer.Domain.Models;

namespace Consumer.Tests.Services
{
    public class MessageServiceTests
    {
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly IMessageService _messageService;

        public MessageServiceTests()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _messageService = new MessageService(_mockMessageRepository.Object);
        }

        [Fact]
        public async Task ProcessMessageAsync_ShouldSaveToMongo()
        {
            // Arrange
            var message = "test message";

            _mockMessageRepository
                .Setup(r => r.InsertAsync(It.IsAny<ConsumerModel>()))
                .Returns(Task.CompletedTask);

            // Act
            await _messageService.ProcessMessageAsync(message);

            // Assert
            _mockMessageRepository.Verify(r => r.InsertAsync(It.IsAny<ConsumerModel>()), Times.Once);
        }
    }
}
