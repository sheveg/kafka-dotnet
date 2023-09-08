using CoreLibrary;
using KafkaProducer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection(KafkaOptions.Kafka));
builder.Services.AddHostedService<Producer>();

var host = builder.Build();
host.Run();