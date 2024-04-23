using System;
using common;


// See https://aka.ms/new-console-template for more information
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("****Send Messages*****");
var rabbitMqClient = new MessageQueue();

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
    "`EAT",
    "ME'",
    "were",
    "beautifully",
    "marked",
    "in",
    "currants"})
{
    var message = string.Concat("Word ", word, " ", DateTime.Now);

    rabbitMqClient.Publish(word);

    Console.WriteLine("Send Message = {0}", word);
}

Console.ReadKey();