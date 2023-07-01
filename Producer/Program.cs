using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args) // We don't have to use Main https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/top-level-statements
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "message-queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var count = 1;

            while (true)
            {
                var message = $"Message #{count}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: string.Empty, // equals to default exchange
                     routingKey: "message-queue", // queue name
                     basicProperties: null,
                     body: body);

                Console.WriteLine($"Producer sent '{message}'");

                count++;
                Thread.Sleep(2000);
            }
        }
    }
}
