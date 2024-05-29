using Consumer.Domain.Models;

namespace Consumer.Infrastructure.Interfaces
{
    public interface IMessageRepository
    {
        Task InsertAsync(ConsumerModel model);
    }
}
