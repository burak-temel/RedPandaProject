
namespace Producer.Services
{
    public interface IRedPandaService<TRequest, TResponse>
    {
        void Dispose();
        Task PublishRequestAsync(TRequest request, string topic);
    }
}