using Confluent.Kafka;
using CoreLibrary;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaConsumer
{
    public class Consumer : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly string _topic;
        private readonly IConsumer<Null, string> _consumer;

        public Consumer(ILogger<Consumer> logger, IOptions<KafkaOptions> kafkaOptions)
        {
            _logger = logger;
            _topic = kafkaOptions.Value.Topic;

            _consumer = new ConsumerBuilder<Null, string>(
                new ConsumerConfig { 
                    BootstrapServers = kafkaOptions.Value.BootstrapUrl,
                    GroupId = "foo"
                })
                .Build();

            _consumer.Subscribe(kafkaOptions.Value.Topic);
            _logger.LogInformation("Consuming on url {url} and topic {topic}", kafkaOptions.Value.BootstrapUrl, _topic);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var randomNumber = Random.Shared.Next(100);
                _consumer.Consume(1000);
                _logger.LogInformation("Produced message with number {randomNumber}.", randomNumber);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
