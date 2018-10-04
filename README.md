# SteBakerDev.RabbitMQ.NetCore
A NuGet package using the eShop on containers implementation of RabbitMQ with some added helper methods

[Source](https://github.com/dotnet-architecture/eShopOnContainers/)


## Usage (AspNetCore)

- In Startup.cs, add the following to the ConfigureServices method:
``` cs
    services.RegisterEventBus("MyMqExchangeName", "MyApplicationQueueName");
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

    eventBus.Subscribe<CustomerUpdatedIntegrationEvent, CustomerUpdatedIntegrationEventHandler>();

CustomerUpdatedIntegrationEventHandler.cs

    public class CustomerUpdatedIntegrationEventHandler : IIntegrationEventHandler<CustomerUpdatedEvent>
    {
        public async Task Handle(CustomerUpdatedEvent @event)
        {
            Console.WriteLine("Received Event");
        }
    }

```