using System;
using common;
using System.Threading;


// See https://aka.ms/new-console-template for more information
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("****Send Messages*****");
var rabbitMqClient = new MessageQueue();

var task = Task.Run(() 
    => Enumerable.Range(0, 200)
    .ToList()
    .ForEach(i
        => 
        {
            Thread.Sleep(1000);

            Console.WriteLine("Send Time = {0}", i);

            rabbitMqClient
                .Publish(
                    rabbitMqClient.QueueName2,
                    DateTime.Now.ToString());
        }));
    
foreach(var word in new[] {"several",
    "Alice",
    "possibly",
    "Soon",
    "her",
    "eye",
    "fell",
    "on",
    "a",
    "little",
    "glass",
    "box",
    "that",
    "was",
    "lying",
    "under",
    "the",
    "table:",
    "she",
    "opened",
    "it,",
    "and",
    "found",
    "in",
    "it",
    "a",
    "very",
    "small",
    "cake,",
    "on",
    "which",
    "the",
    "words",
    "EAT",
    "ME'",
    "were",
    "beautifully",
    "marked",
    "in",
    "currants"})
{
    var message = string.Concat("Word ", word, " ", DateTime.Now);

    rabbitMqClient.Publish(rabbitMqClient.QueueName1, word);

    Console.WriteLine("Send Message = {0}", word);
}

task.Wait();

Console.ReadKey();