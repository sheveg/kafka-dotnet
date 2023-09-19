using Confluent.Kafka;
using CoreLibrary;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace KafkaProducer
{
    public class Producer : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;

        public Producer(ILogger<Producer> logger, IOptions<KafkaOptions> kafkaOptions) 
        {
            _logger = logger;
            _topic = kafkaOptions.Value.Topic;

            _producer = new ProducerBuilder<Null, string>(
                new ProducerConfig { BootstrapServers = kafkaOptions.Value.BootstrapUrl })
                .Build();

            _logger.LogInformation("Writing on url {url} and topic {topic}", kafkaOptions.Value.BootstrapUrl, _topic);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var randomNumber = Random.Shared.Next(100);
                var message = JsonSerializer.Serialize(new RandomMessage { Id = randomNumber, Message = "Message produced." });
                _producer.Produce(_topic, new Message<Null, string> { Value = message });
                _logger.LogInformation("Produced message with number {randomNumber}.", randomNumber);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
