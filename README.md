# SteBakerDev.RabbitMQ.NetCore
A NuGet package using the eShop on containers implementation of RabbitMQ with some added helper methods

[Source](https://github.com/dotnet-architecture/eShopOnContainers/)


## Usage (AspNetCore)

- In Startup.cs, add the following to the ConfigureServices method:
``` cs
    services.RegisterEventBus("MyExchange", "MyApplication");
```

- If your application needs to subscribe to events, add the following:

``` cs
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<CustomerUpdatedIntegrationEvent, CustomerUpdatedIntegrationEventHandler>();
```

- To send events, inject the IEventBus instance and call the Publish method:
- 
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