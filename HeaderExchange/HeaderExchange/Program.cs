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

channel.ExchangeDeclare("ex.headers", "headers", true, false, null);

channel.QueueDeclare("my.queueHeader1", true, false, false, null);
channel.QueueDeclare("my.queueHeader2", true, false, false, null);

channel.QueueBind(
    "my.queueHeader1",
    "ex.headers",
    "",
    new Dictionary<string, object>()
    {
        { "x-match", "all" },
        { "job", "convert" },
        { "format", "jpeg" }
    }
);

channel.QueueBind(
    "my.queueHeader2",
    "ex.headers",
    "",
    new Dictionary<string, object>()
    {
        { "x-match", "any" },
        { "job", "convert" },
        { "format", "bmp" }
    }
);

IBasicProperties properties = channel.CreateBasicProperties();
properties.Headers = new Dictionary<string, object>
{
    { "job", "convert" },
    { "format", "jpeg" }
};
channel.BasicPublish("ex.headers", "", properties, Encoding.UTF8.GetBytes("Message 1"));

properties = channel.CreateBasicProperties();
properties.Headers = new Dictionary<string, object>
{
    { "job", "convert" },
    { "format", "bmp" }
};
channel.BasicPublish("ex.headers", "", properties, Encoding.UTF8.GetBytes("Message 2"));