using Producer.Domain.Models;
using System.Threading.Tasks;

namespace Producer.Infrastructure.Interfaces
{
    public interface IMessageRepository
    {
        Task InsertAsync(ProducerModel model);
    }
}
