namespace Consumer.Infrastructure.Settings
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
        public string ResponseTopic { get; set; }
    }
}
