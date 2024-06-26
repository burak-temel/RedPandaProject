using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Producer.Application.Interfaces;
using Producer.Application.Services;
using Producer.Infrastructure.Interfaces;
using Producer.Infrastructure.Repositories;
using Producer.Infrastructure.Settings;
using Producer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

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
builder.Services.AddSingleton(typeof(IRedPandaService<,>), typeof(RedPandaService<,>));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kafka Producer configuration
builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var kafkaSettings = builder.Configuration.GetSection("Kafka").Get<KafkaSettings>();
    var config = new ProducerConfig
    {
        BootstrapServers = kafkaSettings.BootstrapServers
    };

    return new ProducerBuilder<Null, string>(config).Build();
});

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
