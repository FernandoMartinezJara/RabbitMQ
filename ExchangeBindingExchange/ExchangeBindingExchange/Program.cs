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

channel.ExchangeDeclare("exchange1", "direct", true, false, null);
channel.ExchangeDeclare("exchange2", "direct", true, false, null);

channel.QueueDeclare("queue1", true, false, false, null);
channel.QueueDeclare("queue2", true, false, false, null);

channel.QueueBind("queue1", "exchange1", "key1");
channel.QueueBind("queue2", "exchange2", "key2");

channel.ExchangeBind("exchange2", "exchange1", "key2");

channel.BasicPublish("exchange1", "key1", null, Encoding.UTF8.GetBytes("Message 1 from exchange1 on key1"));
channel.BasicPublish("exchange1", "key2", null, Encoding.UTF8.GetBytes("Message 1 from exchange1 on ke2"));
