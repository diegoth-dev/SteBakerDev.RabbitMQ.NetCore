# SteBakerDev.RabbitMQ.NetCore
A NuGet package using the eShop on containers implementation of RabbitMQ with some added helper methods

[Source](https://github.com/dotnet-architecture/eShopOnContainers/)


## Usage (AspNetCore)

- In Startup.cs, add the following to the ConfigureServices method:
```
    services.RegisterEventBus("MyExchange", "MyApplication");
```

- If your application needs to subscribe to events, add the following:

```
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<CustomerUpdatedIntegrationEvent, CustomerUpdatedIntegrationEventHandler>();
```