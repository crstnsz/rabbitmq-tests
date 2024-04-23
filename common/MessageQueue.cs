using System.Runtime.Intrinsics.X86;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace common;
public class MessageQueue : IDisposable
{
    private readonly string _connectionString = "amqp://guest:guest@localhost:5672";
    private readonly string queueName = "my-queue";
    private readonly IConnection connection;
    private readonly IModel model;

    public MessageQueue()
    {
        connection = new ConnectionFactory
        {
            Uri = new Uri(_connectionString)
        }
        .CreateConnection();
        model = connection.CreateModel();
    }

    private void ConfigureQueue(string name, IModel model)
    {
        model.QueueDeclare(queue: name, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Publish(string message)
    {
        ConfigureQueue(queueName, model);
        model.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: Encoding.UTF8.GetBytes(message));
    }

    public void Listen()
    {
        ConfigureQueue(queueName, model);

        var consumer = new EventingBasicConsumer(model);
        consumer.Received += (sender, e) =>
        {
            var  pre = DateTime.Now;
            var body = e.Body.ToArray();
            var word = Encoding.UTF8.GetString(body);

            if (!string.IsNullOrWhiteSpace(word))
            {
                model.BasicAck(e.DeliveryTag, true);
            }

            var count = File.ReadAllText(@"D:\git\rabbit-mq\alice_in_wonderland.txt")
                .Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Count(x => string.Compare(x, word, true) == 0);

            Console.WriteLine($"Received Message: Word {word} count {count} {pre} {DateTime.Now}");
        };

        model.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void Dispose()
    {
        if (model != null && model.IsOpen)
        {
            model.Close();
            model.Dispose();
        }

        if (connection != null && connection.IsOpen)
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
