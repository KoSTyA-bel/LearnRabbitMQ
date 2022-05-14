using RabbitMQ.Client;
using System.Text;

namespace Send.HelloWorld;

internal class Send
{
    private ConnectionFactory connectionFactory;

    public Send(string hostName = "127.0.0.1", int port = 5672)
    {
        connectionFactory = new ConnectionFactory() { HostName = hostName, Port = port, UserName = "guest", Password = "guest"};
    }

    public string Queue { get; init; } = "hello";

    public void SendMessage(string message = "Hello World!")
    {
        using (var connection = connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: Queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: Queue,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("Message sended");
        }
    }
}
