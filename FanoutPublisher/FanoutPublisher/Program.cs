using System.Text;
using RabbitMQ.Client;

IConnection conn;
IModel channel;

ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.VirtualHost = "/";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";

conn = factory.CreateConnection();
channel = conn.CreateModel();

channel.ExchangeDeclare(
    "ex.fanout",
    "fanout",
    true,
    false,
    null);

channel.QueueDeclare("my.queue1", true, false, false, null);
channel.QueueDeclare("my.queue2", true, false, false, null);

channel.QueueBind("my.queue1", "ex.fanout", "");
channel.QueueBind("my.queue2", "ex.fanout", "");

channel.BasicPublish(
    "ex.fanout",
    "",
    null,
    Encoding.UTF8.GetBytes("Message 1 from .NET"));

channel.BasicPublish(
    "ex.fanout",
    "",
    null,
    Encoding.UTF8.GetBytes("Message 2 .NET"));

Console.WriteLine("press any key to exit");
Console.ReadKey();

channel.QueueDelete("my.queue1");
channel.QueueDelete("my.queue2");

channel.ExchangeDelete("ex.fanout");

channel.Close();
conn.Close();