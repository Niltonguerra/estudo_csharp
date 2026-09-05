using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationsWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "product.created";

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _config["RabbitMQ:Host"] ?? "rabbitmq",
            UserName = _config["RabbitMQ:Username"] ?? "guest",
            Password = _config["RabbitMQ:Password"] ?? "guest"
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken
        );

        _logger.LogInformation("Worker conectado ao RabbitMQ, aguardando mensagens...");

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel!);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Produto criado recebido: {Json}", json);

            // Aqui você processaria o evento — enviar email, notificação push, etc.

            await _channel!.BasicAckAsync(args.DeliveryTag, multiple: false);
        };

        await _channel!.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );

        // Mantém o worker rodando
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        if (_channel is not null)
            await _channel.DisposeAsync();

        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}