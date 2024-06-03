namespace Consumer.Services
{
    public interface IRedPandaService<TRequest, TResponse>
    {
        Task PublishRequestAsync(TRequest request, string topic);
        TRequest ConsumeMessage(string topic, CancellationToken cancellationToken);
        void Dispose();
    }

}
