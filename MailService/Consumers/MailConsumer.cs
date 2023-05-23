using Confluent.Kafka;

public class MailConsumer
{
    private readonly ILogger<MailConsumer> _logger;
    private readonly IConsumer<Ignore, string> _consumer;

    public MailConsumer(ILogger<MailConsumer> logger)
    {
        _logger = logger;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "my-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Ignore, string>(config)
            .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
            .Build();
    }

    public void Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
        while (true)
        {
            try
            {
                var result = _consumer.Consume();
                _logger.LogInformation($"Received message at {result.TopicPartitionOffset}: {result.Value}");
            }
            catch (ConsumeException e)
            {
                _logger.LogError($"Error while consuming message: {e.Error.Reason}");
            }
        }
    }

    public void Close()
    {
        _consumer.Close();
    }
}
