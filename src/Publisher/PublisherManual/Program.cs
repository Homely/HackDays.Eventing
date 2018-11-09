using Newtonsoft.Json;
using PublisherManual.Events;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace PublisherManual
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Manual Publish...");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "listings", type: "fanout");
                        
                    while (true)
                    {
                        double eventType = 0;
                        while (eventType == 0)
                        {
                            Console.Write("Event type to publish [Created == 1 | Updated = 2]: ");
                            var eventTypeKey = Console.ReadKey();
                            if (eventTypeKey.Key != ConsoleKey.D1 &&
                                eventTypeKey.Key != ConsoleKey.D2)
                            {
                                Console.WriteLine();
                                Console.WriteLine("   !!! Invalid option !!!");
                            }
                            else
                            {
                                Console.WriteLine();
                                eventType = char.GetNumericValue(eventTypeKey.KeyChar);
                            }
                        }

                        Console.Write("Message body to publish: ");
                        var message = Console.ReadLine();

                        var listingCreatedEvent = new ListingCreatedOrUpdatedEvent
                        {
                            ListingId = 11,
                            EventType = eventType == 1 ? EventType.Created : EventType.Updated,
                            Title = message
                        };

                        var json = JsonConvert.SerializeObject(listingCreatedEvent);
                        var body = Encoding.UTF8.GetBytes(json);

                        channel.BasicPublish(exchange: "listings",
                                             routingKey: "",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($" [{DateTime.UtcNow}] Sent {json}");

                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
