using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
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

            channel.BasicQos
            (
                prefetchSize: 0, // message size limit; 0 - no limit
                prefetchCount: 2,
                global: false // false - per consumer limit; true - per channel limit
            );

            Console.WriteLine("Consumer waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Consumer received {message}. Thread ID {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                //Run each received message in a separate task(thread)
                //Task.Run(() =>
                //{
                //    var body = ea.Body.ToArray();
                //    var message = Encoding.UTF8.GetString(body);
                //    Console.WriteLine($"Consumer received {message}. Thread ID {Thread.CurrentThread.ManagedThreadId}");
                //    Thread.Sleep(2000);
                //    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                //});
            };
            channel.BasicConsume
            (
                queue: "message-queue",
                autoAck: false,
                consumer: consumer
            );

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
