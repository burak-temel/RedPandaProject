using Confluent.Kafka;
using Consumer.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Consumer.Services
{
    public class RedPandaService<TRequest, TResponse> : IDisposable
    {
        private readonly IConsumer<Ignore, string> _consumer;

        public RedPandaService(IOptions<KafkaSettings> kafkaSettings)
        {
            var consumerConfig = new ConsumerConfig
            {
                GroupId = kafkaSettings.Value.GroupId,
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        public TRequest ConsumeMessage(string topic, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                if (consumeResult != null)
                {
                    var request = JsonConvert.DeserializeObject<TRequest>(consumeResult.Message.Value);
                    return request;
                }
            }

            return default;
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
