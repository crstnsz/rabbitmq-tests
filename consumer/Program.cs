using common;
using System;
using System.Text;



Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("****Receive Messages*****");

var rabbitMqClient = new MessageQueue();

Console.WriteLine($"Listen {rabbitMqClient.QueueName1}");

rabbitMqClient
    .Listen(rabbitMqClient.QueueName1,
    (sender, e) =>
    {
        var  pre = DateTime.Now;
        var body = e.Body.ToArray();
        var word = Encoding.UTF8.GetString(body);

        if (!string.IsNullOrWhiteSpace(word))
        {
            rabbitMqClient.Ack(
                rabbitMqClient.QueueName1,
                e.DeliveryTag);
        }

        var count = File.ReadAllText(@"D:\git\rabbit-mq\alice_in_wonderland.txt")
            .Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Count(x => string.Compare(x, word, true) == 0);

        Console.WriteLine($"Received Message: Word {word} count {count} {pre} {DateTime.Now}");
    });

Console.WriteLine($"Listen {rabbitMqClient.QueueName2}");


rabbitMqClient
    .Listen(rabbitMqClient.QueueName2,
    (sender, e) =>
    {
        var body = e.Body.ToArray();
        var time = Encoding.UTF8.GetString(body);

        if (!string.IsNullOrWhiteSpace(time))
        {
            rabbitMqClient.Ack(
                rabbitMqClient.QueueName2,
                e.DeliveryTag);
        }

        Console.WriteLine($"Received Message: Time {time}");
    });

Console.ReadKey();