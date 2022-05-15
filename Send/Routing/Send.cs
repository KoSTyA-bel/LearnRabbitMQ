using RabbitMQ.Client;
using System.Text;

namespace Send.Routing;

internal class Send
{
    private ConnectionFactory connectionFactory;

    public Send(string hostName = "127.0.0.1", int port = 5672)
    {
        connectionFactory = new ConnectionFactory() { HostName = hostName, Port = port, UserName = "guest", Password = "guest"};
    }

    public string Exchange { get; set; } = "logs";

    public void SendMessage(string message = "Hello World!")
    {
        using (var connection = connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(Exchange, ExchangeType.Direct);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: Exchange,
                                 routingKey: "critical",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("Message sended");
        }
    }
}
