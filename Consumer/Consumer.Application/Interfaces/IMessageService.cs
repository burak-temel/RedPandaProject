namespace Consumer.Application.Interfaces
{
    public interface IMessageService
    {
        Task ProcessMessageAsync(string message);
    }
}
