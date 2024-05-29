using System.Threading.Tasks;
using Moq;
using Xunit;
using Confluent.Kafka;
using Producer.Application.Interfaces;
using Producer.Application.Services;
using Producer.Infrastructure.Interfaces;
using Producer.Domain.Models;

namespace Producer.Tests.Services
{
    public class MessageServiceTests
    {
        private readonly Mock<IProducer<Null, string>> _mockProducer;
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly IMessageService _messageService;

        public MessageServiceTests()
        {
            _mockProducer = new Mock<IProducer<Null, string>>();
            _mockMessageRepository = new Mock<IMessageRepository>();
            _messageService = new MessageService(_mockProducer.Object, _mockMessageRepository.Object);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendToKafkaAndSaveToMongo()
        {
            // Arrange
            var message = "test message";
            var deliveryResult = new DeliveryResult<Null, string> { Offset = new Offset(0) };

            _mockProducer
                .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>()))
                .ReturnsAsync(deliveryResult);

            _mockMessageRepository
                .Setup(r => r.InsertAsync(It.IsAny<ProducerModel>()))
                .Returns(Task.CompletedTask);

            // Act
            await _messageService.SendMessageAsync(message);

            // Assert
            _mockProducer.Verify(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>()), Times.Once);
            _mockMessageRepository.Verify(r => r.InsertAsync(It.IsAny<ProducerModel>()), Times.Once);
        }
    }
}
