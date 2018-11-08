using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    while (true)
                    {
                        channel.BasicPublish(exchange: "",
                                             routingKey: "hello",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}", message);

                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
