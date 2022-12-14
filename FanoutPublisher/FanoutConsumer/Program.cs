using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

var consumer = new EventingBasicConsumer(channel);
consumer.Received += Consumer_Received;

var consumerTag = channel.BasicConsume("my.queue2", false, consumer);

Console.WriteLine("Waiting for messages. press any key to exit");
Console.ReadKey();

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    string message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine(message);

    //channel.BasicAck(e.DeliveryTag, false);
    //channel.BasicNack(e.DeliveryTag, false, true);
}