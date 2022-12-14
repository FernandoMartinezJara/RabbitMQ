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


channel.QueueDeclare("my.Defaultqueue1", true, false, false, null);
channel.QueueDeclare("my.Defaultqueue2", true, false, false, null);

channel.BasicPublish("", "my.Defaultqueue1", null, Encoding.UTF8.GetBytes("Message 1 with routing key my.Defaultqueue1"));
channel.BasicPublish("", "my.Defaultqueue1", null, Encoding.UTF8.GetBytes("Message 2 with routing key my.Defaultqueue1"));
channel.BasicPublish("", "my.Defaultqueue2", null, Encoding.UTF8.GetBytes("Message 3 with routing key my.Defaultqueue2"));