using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Recive.Topic;

internal class Recive
{
    private ConnectionFactory connectionFactory;

    public Recive(string hostName = "127.0.0.1", int port = 5672)
    {
        connectionFactory = new ConnectionFactory() { HostName = hostName, Port = port };
    }

    public string Name { get; set; } = string.Empty;
    public string Exchange { get; set; } = "Topic";
    public List<string> Topics { get; init; } = new();

    public void ReciveMessage()
    {
        using (var connection = connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(Exchange, ExchangeType.Topic);
            var quaueName = channel.QueueDeclare().QueueName;

            foreach (var topic in Topics)
            {
                channel.QueueBind(quaueName, Exchange, topic);
            }            

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"{Name} recived a message: {message}");
            };

            channel.BasicConsume(queue: quaueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.ReadLine();
        }
    }
}
