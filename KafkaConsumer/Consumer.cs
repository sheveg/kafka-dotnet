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
using static System.Net.Mime.MediaTypeNames;

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
                    SaslUsername = kafkaOptions.Value.User,
                    SaslPassword = kafkaOptions.Value.Password,
                    SaslMechanism = kafkaOptions.Value.SaslMechanismEnum,
                    SecurityProtocol = kafkaOptions.Value.SecurityProtocolEnum,
                    GroupId = "foo"
                })
                .SetLogHandler((_, logMessage) => _logger.LogInformation("Kafka log: {Message}", logMessage.Message))
                .SetErrorHandler((_, error) =>
                {
                    if (error.IsFatal)
                    {
                        _logger.LogError("Kafka fatal error: {Reason}", error.Reason);
                    }
                    _logger.LogWarning("Kafka error: {Reason}", error.Reason);
                })
                .Build();

            _consumer.Subscribe(kafkaOptions.Value.Topic);
            _logger.LogInformation("Consuming on url {url} and topic {topic}", kafkaOptions.Value.BootstrapUrl, _topic);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _consumer.Consume(1000);
                _logger.LogInformation("Consumed message: {message}.", message?.Message?.Value);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
