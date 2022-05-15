using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Recive.Exchange;

internal class Recive
{
    private ConnectionFactory connectionFactory;

    public Recive(string hostName = "127.0.0.1", int port = 5672)
    {
        connectionFactory = new ConnectionFactory() { HostName = hostName, Port = port };
    }

    public string Exchange { get; set; } = "logs";

    public void ReciveMessage()
    {
        using (var connection = connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(Exchange, ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, Exchange, string.Empty);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);

                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);

                Console.WriteLine("Done");
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.ReadLine();
        }
    }
}
