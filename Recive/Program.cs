using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() {
    HostName = "127.0.0.1",
    Port = 5672,
    UserName = "guest",
    Password = "guest" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "rpc_queue", durable: false,
      exclusive: false, autoDelete: false, arguments: null);
    channel.BasicQos(0, 1, false);
    var consumer = new EventingBasicConsumer(channel);
    channel.BasicConsume(queue: "rpc_queue",
      autoAck: false, consumer: consumer);
    Console.WriteLine(" [x] Awaiting RPC requests");

    consumer.Received += (model, ea) =>
    {
        string response = null;

        var body = ea.Body.ToArray();
        var props = ea.BasicProperties;
        var replyProps = channel.CreateBasicProperties();
        replyProps.CorrelationId = props.CorrelationId;

        try
        {
            var message = Encoding.UTF8.GetString(body);
            int n = int.Parse(message);
            Console.WriteLine(" [.] Fact({0})", message);
            response = Fact(n).ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(" [.] " + e.Message);
            response = "";
        }
        finally
        {
            var responseBytes = Encoding.UTF8.GetBytes(response);
            channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
              basicProperties: replyProps, body: responseBytes);
            channel.BasicAck(deliveryTag: ea.DeliveryTag,
              multiple: false);
        }
    };

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();

    static int Fact(int n) => n == 0 ? 1 : n * Fact(n - 1);
}