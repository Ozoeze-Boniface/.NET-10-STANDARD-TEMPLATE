using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using CityCode.MandateSystem.Domain.Common;

var builder = WebApplication.CreateBuilder(args);

// builder.Host.UseSerilog(
//     (HostBuilderContext context, IServiceProvider serviceProvider, LoggerConfiguration config) => config.ReadFrom
//         .Configuration(context.Configuration)
//         .ReadFrom.Services(serviceProvider));


// builder.Services.AddApplicationServices();

builder.Services.Configure<HostOptions>(x =>
  {
    x.ServicesStartConcurrently = true;
    x.ServicesStopConcurrently = false;

  });

builder.Services.AddWorkerServices(builder.Configuration);
var kafkaSettings = builder.Configuration.GetSection("KafkaSettings").Get<KafkaSettings>();
builder.Host.UseSerilog();

var consumerConfig = new ConsumerConfig
{

  BootstrapServers = kafkaSettings!.BootstrapServers,
  GroupId = kafkaSettings.GroupId,
  AutoOffsetReset = AutoOffsetReset.Earliest

};
builder.Services.AddSingleton(consumerConfig);

var topic = kafkaSettings.Topic;

builder.Services.AddSingleton(topic);
builder.Services.AddHostedService<ScheduleUpdateWorker>();
// builder.Services.AddHostedService<SampleWorker>();
// builder.Services.AddApplicationServices();
// builder.Services.AddInfrastructureServices(builder.Configuration);



var app = builder.Build();


app.UseSerilogRequestLogging();

app.Run();
