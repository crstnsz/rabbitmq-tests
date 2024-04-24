using System.Runtime.Intrinsics.X86;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace common;
public class MessageQueue : IDisposable
{
    private readonly string _connectionString = "amqp://guest:guest@localhost:5672";
    public readonly string QueueName1 = "my-queue-1";
    public readonly string QueueName2 = "my-queue-2";
    private readonly IConnection connection;
    private readonly IModel model1;
    private readonly IModel model2;

    public void Ack(string queueName, ulong deliveryTag)
        =>GetModel(queueName)
        .BasicAck(deliveryTag, true);

    public MessageQueue()
    {
        connection = new ConnectionFactory
        {
            Uri = new Uri(_connectionString)
        }
        .CreateConnection();
        model1 = connection.CreateModel();
        model2 = connection.CreateModel();
    }

    private void ConfigureQueue(string name, IModel model)
    {
        GetModel(name).QueueDeclare(queue: name, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    private IModel GetModel(string queueName)
        => queueName==QueueName1
        ? model1
        : model2;

    public void Publish(string queueName, string message)
    {
        var model = GetModel(queueName);

        ConfigureQueue(queueName, model);
        model.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: Encoding.UTF8.GetBytes(message));
    }

    public void Listen(string queueName, EventHandler<BasicDeliverEventArgs> eventHandler)
    {
        var model = GetModel(queueName);

        ConfigureQueue(queueName, model);

        var consumer = new EventingBasicConsumer(model);
        consumer.Received += eventHandler;

        model.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void Dispose()
    {

        if (model1 != null && model1.IsOpen)
        {
            model1.Close();
            model1.Dispose();
        }

        if (model2 != null && model2.IsOpen)
        {
            model2.Close();
            model2.Dispose();
        }

        if (connection != null && connection.IsOpen)
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
