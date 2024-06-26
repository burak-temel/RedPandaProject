using Consumer.Domain.Models;

namespace Consumer.Infrastructure.Interfaces
{
    public interface IMessageRepository
    {
        Task SaveMessageAsync(ConsumerModel model);
    }
}
