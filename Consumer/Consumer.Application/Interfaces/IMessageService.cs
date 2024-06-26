using Common.Models;
using Consumer.Application.DTOs;
using Consumer.Domain.Models;

namespace Consumer.Application.Interfaces
{
    public interface IMessageService
    {
        Task ProcessMessageAsync(MessageDTO message);
        Task ProcessMessageAsync(ConsumerDTO message);
    }
}
