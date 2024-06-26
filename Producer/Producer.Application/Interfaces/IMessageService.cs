using Producer.Application.NewFolder;
using Producer.Domain.Models;

namespace Producer.Application.Interfaces
{
    public interface IMessageService
    {
        Task SendMessageAsync(ProducerDTO model);
    }
}
