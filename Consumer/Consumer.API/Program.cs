using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Consumer.Application.Interfaces;
using Consumer.Application.Services;
using Consumer.Infrastructure.Interfaces;
using Consumer.Infrastructure.Repositories;
using Consumer.Infrastructure.Settings;
using Consumer.API.Workers;
using Consumer.Services;
using Common.Models;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection(nameof(KafkaSettings)));

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return sp.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName);
});

builder.Services.AddSingleton<IMessageRepository, MessageRepository>();
builder.Services.AddSingleton<IMessageService, MessageService>();

builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var config = new ProducerConfig { BootstrapServers = builder.Configuration["KafkaSettings:BootstrapServers"] };
    return new ProducerBuilder<Null, string>(config).Build();
});

builder.Services.AddSingleton<IConsumer<Ignore, string>>(sp =>
{
    var config = new ConsumerConfig
    {
        GroupId = builder.Configuration["KafkaSettings:GroupId"],
        BootstrapServers = builder.Configuration["KafkaSettings:BootstrapServers"],
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new ConsumerBuilder<Ignore, string>(config).Build();
});

// Register RedPandaService with generics
builder.Services.AddSingleton(typeof(IRedPandaService<,>), typeof(RedPandaService<,>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kafka Consumer configuration
builder.Services.AddHostedService<ConsumerWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
