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

channel.ExchangeDeclare("ex.direct", "direct", true, false, null);

channel.QueueDeclare("my.infos", true, false, false, null);
channel.QueueDeclare("my.warnings", true, false, false, null);
channel.QueueDeclare("my.errors", true, false, false, null);


channel.QueueBind("my.infos", "ex.direct", "info");
channel.QueueBind("my.warnings", "ex.direct", "warning");
channel.QueueBind("my.errors", "ex.direct", "error");

channel.BasicPublish("ex.direct", "info", null, Encoding.UTF8.GetBytes("Messages wit routing key info"));
channel.BasicPublish("ex.direct", "warning", null, Encoding.UTF8.GetBytes("Messages wit routing key warning"));
channel.BasicPublish("ex.direct", "error", null, Encoding.UTF8.GetBytes("Messages wit routing key error"));