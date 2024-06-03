using Confluent.Kafka;
using Consumer.Infrastructure.Settings;
using Consumer.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Consumer.API.Workers
{
    public abstract class BaseWorker<TRequest> : BackgroundService
    {
        private readonly string _topic;
        private readonly IRedPandaService<TRequest, string> _redPandaService;

        protected BaseWorker(string topic, IRedPandaService<TRequest, string> redPandaService)
        {
            _topic = topic;
            _redPandaService = redPandaService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _redPandaService.ConsumeMessage(_topic, stoppingToken);
                if (message != null)
                {
                    await ProcessMessageAsync(message);
                }
            }
        }

        protected abstract Task ProcessMessageAsync(TRequest message);
    }
}
