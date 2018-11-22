# SteBakerDev.RabbitMQ.NetCore
A NuGet package based off the Microsoft eShop on containers implementation of RabbitMQ with some small improvements & added helper methods

[Source](https://github.com/dotnet-architecture/eShopOnContainers/)

## Features

- Minimal setup for subscribing and handling events

## Usage (AspNetCore)

- In Startup.cs, add the following to the ConfigureServices method to create a persistent connection & configure / inject the IEventBus implementation . `queueName` is only required when subscribing to events
``` cs
    // Create a persistent connection
    services.AddRabbitMQPersistentConnection("hostname", "username", "password", retryCount: 5);

    // Setup the IEventBus and Exchange. QueueName is only required when subscribing to events  
    services.RegisterEventBus("MyMqExchangeName", queueName: "MyApplicationQueueName");
```

- To send events, inject the IEventBus instance and call the Publish method:
``` cs
    private readonly IEventBus _eventBus;

    public MyService(IEventBus eventBus)
    {
        _eventBus = eventBus;    
    }
    ...
    public void Send(int customerId)
    {
        // Publish an event to MyExchange
        _eventBus.Publish(new CustomerUpdatedEvent(customerId));
    }
```

- If your application needs to subscribe to events, create an EventHandler and add the following to Configure method:
``` cs
// Startup.cs

    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<CustomerUpdatedEvent, CustomerUpdatedEventHandler>();

...

// CustomerUpdatedIntegrationEventHandler.cs

    public class CustomerUpdatedEventHandler : IIntegrationEventHandler<CustomerUpdatedEvent>
    {
        public async Task Handle(CustomerUpdatedEvent @event)
        {
            Console.WriteLine("Received a CustomerUpdatedEvent");
        }
    }

```

## TODO

- [ ] Setup a CI server to publish NuGet packages

