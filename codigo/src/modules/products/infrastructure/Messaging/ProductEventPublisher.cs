using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ProductsApi.Modules.Products.Domain.Events;
using ProductsApi.Modules.Products.Application.Services;
using ProductsApi.Modules.Products.Domain.Interfaces;
using ProductsApi.Modules.Products.Infrastructure.Messaging;
using ProductsApi.Modules.Products.Infrastructure.Persistence.Repositories;


namespace ProductsApi.Modules.Products.Infrastructure.Messaging;

public class ProductEventPublisher : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private const string QueueName = "product.created";

    public interface IProductEventPublisher
    {
        Task PublishProductCreatedAsync(ProductCreatedEvent productEvent);
    }

    private ProductEventPublisher(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public static async Task<ProductEventPublisher> CreateAsync(IConfiguration config)
    {
        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQ:Host"] ?? "rabbitmq",
            UserName = config["RabbitMQ:Username"] ?? "guest",
            Password = config["RabbitMQ:Password"] ?? "guest"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        return new ProductEventPublisher(connection, channel);
    }

    public async Task PublishProductCreatedAsync(ProductCreatedEvent productEvent)
    {
        var json = JsonSerializer.Serialize(productEvent);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: QueueName,
            body: body
        );
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}