using System.Text;
using RabbitMQ.Client;

IConnection conn;
IModel channel;

ConnectionFactory factory = new()
{
    HostName = "localhost",
    VirtualHost = "/",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

conn = factory.CreateConnection();
channel = conn.CreateModel();

channel.ExchangeDeclare("ex.fanout", "fanout", true, false, null);
channel.ExchangeDeclare("ex.direct", "direct", true, false, new Dictionary<string, object>() { { "alternate-exchange", "ex.fanout" } });

channel.QueueDeclare("my.queue1", true, false, false, null);
channel.QueueDeclare("my.queue2", true, false, false, null);
channel.QueueDeclare("my.unrouted", true, false, false, null);

channel.QueueBind("my.queue1", "ex.direct", "video");
channel.QueueBind("my.queue2", "ex.direct", "image");
channel.QueueBind("my.unrouted", "ex.fanout", "");

channel.BasicPublish("ex.direct", "video", null, Encoding.UTF8.GetBytes("Message with routing key video"));
channel.BasicPublish("ex.direct", "text", null, Encoding.UTF8.GetBytes("Message with routing key text"));