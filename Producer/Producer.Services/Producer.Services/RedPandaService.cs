using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Producer.Infrastructure.Settings;

namespace Producer.Services
{

    public class RedPandaService<TRequest, TResponse> : IRedPandaService<TRequest, TResponse>, IDisposable
    {
        private readonly IProducer<Null, string> _producer;

        public RedPandaService(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task PublishRequestAsync(TRequest request, string topic)
        {
            var jsonMessage = JsonConvert.SerializeObject(request);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}