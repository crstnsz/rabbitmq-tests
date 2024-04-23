using common;
using System;


Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("****Receive Messages*****");

var rabbitMqClient = new MessageQueue();
rabbitMqClient.Listen();
Console.ReadKey();