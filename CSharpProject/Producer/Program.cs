using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare
            (
                queue: "message-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var count = 1;

            while (count <= 10)
            {
                var message = $"Message #{count}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish
                (
                    exchange: string.Empty, // equals to default exchange
                    routingKey: "message-queue", // queue name
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine($"Producer sent '{message}'");

                count++;
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
