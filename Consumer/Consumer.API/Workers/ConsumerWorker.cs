//using Confluent.Kafka;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
//using Consumer.Application.Interfaces;
//using Consumer.Infrastructure.Settings;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;

//public class ConsumerWorker : BackgroundService
//{
//    private readonly IConsumer<Ignore, string> _consumer;
//    private readonly IServiceProvider _serviceProvider;
//    private readonly string _topic = "test-topic";

//    public ConsumerWorker(IOptions<KafkaSettings> kafkaSettings, IServiceProvider serviceProvider)
//    {
//        var config = new ConsumerConfig
//        {
//            GroupId = kafkaSettings.Value.GroupId,
//            BootstrapServers = kafkaSettings.Value.BootstrapServers,
//            AutoOffsetReset = AutoOffsetReset.Earliest
//        };

//        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
//        _serviceProvider = serviceProvider;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        _consumer.Subscribe(_topic);

//        while (!stoppingToken.IsCancellationRequested)
//        {
//            using (var scope = _serviceProvider.CreateScope())
//            {
//                var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

//                try
//                {
//                    var consumeResult = _consumer.Consume(stoppingToken);
//                    if (consumeResult != null)
//                    {
//                        await messageService.ProcessMessageAsync(consumeResult.Message.Value);
//                    }
//                }
//                catch (ConsumeException ex)
//                {
//                    // Handle consumption error
//                }
//            }
//        }
//    }

//    public override void Dispose()
//    {
//        _consumer.Close();
//        _consumer.Dispose();
//        base.Dispose();
//    }
//}


using Common.Models;
using Consumer.Application.Interfaces;
using Consumer.Domain.Models;
using Consumer.Infrastructure.Settings;
using Consumer.Services;
using Microsoft.Extensions.Options;

namespace Consumer.API.Workers
{
    public class ConsumerWorker : BaseWorker<MessageDTO>
    {
        private readonly IMessageService _messageService;

        public ConsumerWorker(IMessageService messageService, IRedPandaService<MessageDTO, string> redPandaService)
            : base("test-topic", redPandaService)
        {
            _messageService = messageService;
        }

        protected override async Task ProcessMessageAsync(MessageDTO message)
        {
            await _messageService.ProcessMessageAsync(message);
        }
    }
}