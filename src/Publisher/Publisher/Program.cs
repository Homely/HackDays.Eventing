using Newtonsoft.Json;
using Publisher.Events;
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
                    channel.ExchangeDeclare(exchange: "listings", type: "fanout");
                    
                    var listingCreatedEvent = new ListingCreatedOrUpdatedEvent
                    {
                        ListingId = 11,
                        Price = 450000,
                        EventType = EventType.Created,
                        Title = "Brand new apartment!"
                    };

                    var msg = JsonConvert.SerializeObject(listingCreatedEvent);
                    var body = Encoding.UTF8.GetBytes(msg);

                    while (true)
                    {
                        channel.BasicPublish(exchange: "listings",
                                             routingKey: "",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}", msg);

                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
