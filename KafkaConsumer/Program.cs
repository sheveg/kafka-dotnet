using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KafkaConsumer;
using CoreLibrary;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection(KafkaOptions.Kafka));
builder.Services.AddHostedService<Consumer>();

var host = builder.Build();
host.Run();