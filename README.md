# HackDays.Eventing
Seeing how we can communicate between independent systems in a microservice architecture, using eventing.

## Setup
1. Install RabbitMQ via docker:
> docker run -d --hostname my-rabbit --name some-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management

2. UI can be viewed via management console:
> http://localhost:15672

## Running
1. Run the `Producer` console app to publish messages
2. Run the `SubscriberOne` and/or `SubscriberTwo` console apps to receive messages