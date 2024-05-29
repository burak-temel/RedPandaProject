namespace Producer.Application.Interfaces
{
    public interface IMessageService
    {
        Task SendMessageAsync(string message);
    }
}
