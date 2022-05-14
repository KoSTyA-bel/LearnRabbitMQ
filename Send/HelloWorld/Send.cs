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

    public string Queue { get; set; } = "hello";

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

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: Queue,
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine("Message sended");
        }
    }
}
