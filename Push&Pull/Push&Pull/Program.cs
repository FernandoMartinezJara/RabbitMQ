using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

//readMessageWithPushMode();

readMessageWithPullMode();

channel.Close();
conn.Close();


void readMessageWithPushMode()
{
    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (sender, e) =>
    {
        string message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine(message);
    };

    string consumerTag = channel.BasicConsume("my.queue1", true, consumer);

    Console.WriteLine("Subscribed, press any key to unSubscribe and exit");
    Console.ReadKey();

    channel.BasicCancel(consumerTag);

}

void readMessageWithPullMode()
{
    Console.WriteLine("Reading messages from queue");

    while (true)
    {
        Console.WriteLine("trying to get message from queue");

        BasicGetResult result = channel.BasicGet("my.queue1", true);

        if (result != null)
        {
            string message = Encoding.UTF8.GetString(result.Body.ToArray());

            Console.WriteLine(message);
        }

        if (Console.KeyAvailable)
        {
            var keyInfo = Console.ReadKey();

            if (keyInfo.KeyChar == 'e' || keyInfo.KeyChar == 'E')
                return;
        }

        Thread.Sleep(2000);
    }


}