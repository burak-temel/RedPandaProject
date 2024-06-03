using Confluent.Kafka;
using Consumer.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Consumer.Services
{
    public class RedPandaService<TRequest, TResponse> : IRedPandaService<TRequest, TResponse>
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IConsumer<Ignore, string> _consumer;

        public RedPandaService(IProducer<Null, string> producer, IConsumer<Ignore, string> consumer)
        {
            _producer = producer;
            _consumer = consumer;
        }

        public async Task PublishRequestAsync(TRequest request, string topic)
        {
            var jsonMessage = JsonConvert.SerializeObject(request);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
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
            _producer.Dispose();
            _consumer.Dispose();
        }
    }
}
