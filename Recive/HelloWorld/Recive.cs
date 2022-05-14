using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Recive.HelloWorld;

internal class Recive
{
    private ConnectionFactory connectionFactory;

    public Recive(string hostName = "127.0.0.1", int port = 5672)
    {
        connectionFactory = new ConnectionFactory() { HostName = hostName, Port = port };
    }

    public string Queue { get; init; } = "hello";

    public void ReciveMessage()
    {
        using (var connection = connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: Queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume(queue: Queue,
                                 autoAck: true,
                                 consumer: consumer);

            Console.ReadLine();
        }
    }
}
