using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace SubscriberTwo
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
                    channel.ExchangeDeclare(exchange: "listings", type: "fanout");

                    var queueName = channel.QueueDeclare(queue: "queuetwo",
                        durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null).QueueName;
                    channel.QueueBind(queue: queueName,
                                      exchange: "listings",
                                      routingKey: "ListingCreatedOrUpdatedEvent");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var obj = JsonConvert.DeserializeObject<ListingUpdatedEvent>(message);
                        Console.WriteLine(" [x] Received {0}", JsonConvert.SerializeObject(obj));
                    };
                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
