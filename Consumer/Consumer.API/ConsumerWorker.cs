using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Consumer.Application.Interfaces;
using Consumer.Infrastructure.Settings;

public class ConsumerWorker : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IMessageService _messageService;
    private readonly string _topic = "test-topic";

    public ConsumerWorker(IOptions<KafkaSettings> kafkaSettings, IMessageService messageService)
    {
        var config = new ConsumerConfig
        {
            GroupId = kafkaSettings.Value.GroupId,
            BootstrapServers = kafkaSettings.Value.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _messageService = messageService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult != null)
                {
                    await _messageService.ProcessMessageAsync(consumeResult.Message.Value);
                }
            }
            catch (ConsumeException ex)
            {
                // Handle consumption error
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}