# Taro Framework

### Bring domain events to your application

## Why I build this?

I had been working on a legacy web application. When I took over the project, I think I need to make some changes to make the codebase more maintainable. Making big changes is risky, so I decided to improve it in small steps.

The first thing I want to change is introducing Domain Events, which is an attractive concept. Cos it's not a large application and we have only one server, so I think the best starting point could be building a Poor Man's event driven framework myself, and that's the first version of Taro.

But there're several design issues in the first version, mostly on the transaction related staff. So here comes the next version of Taro. It's still a Poor Man's event driven framework for small web applications, for which introducing heavy weight frameworks is a burden.

## How to use

### 1. Configuration ##

```csharp
AppRuntime.Instance.Configure(cfg =>
{
    // Use RavenDB (requires Taro.RavenDB.dll)
    cfg.UseRavenDB(documentStore);

    // Run relay worker in current process
    cfg.RunRelayWorkerInCurrentProcess();
})
.Start(); // Start Taro
```

### 2. Define a domain event ###

```csharp
public class OrderApproved : Event
{
    public int OrderId { get; set; }
}
```

Domain events are POCOs inherit from `Taro.Event`.

### 3. Publish events in AggregateRoot ###

```csharp
public class Order : AggregateRoot
{
    public int Id { get; set; }

    public decimal Total { get; set; }

    public void Approve()
    {
        AppendEvent(new OrderApproved
        {
            OrderId = Id
        });
    }
}
```

Aggregate roots are domain models inherit from `Taro.AggregateRoot`. Only aggregate roots are allowed to append domain events (using the `AppendEvent` method). You might wondering why not using `PublishEvent` but `AppendEvent`. This is because domain events will be queued until the db transaction commit.

### 4. Handle events ###

```csharp
public class OrderApproved_NotifyCustomer : IHandles<OrderApproved>
{
    public void Handle(OrderApproved theEvent)
    {
        // Send email to customer...
    }
}
```

### 5. Save aggregates ###

Taro does not have an abstraction on data access. So if using the Taro.RavenDB package, you should use `Taro.IRavenDomainRepository` provied by the Taro.RavenDB package to save aggregates. Repository implementation has already been registerted when calling `cfg.UseRavenDB()` in the configuration step. Following code illustrates how to save the `Order` aggregate:

```csharp
using (var repository = AppRuntime.Instance.CreateDomainRepository<IRavenDomainRepository>())
{
    var order = repository.Find<Order>("orders/1");
    order.Approve();

    repository.Save(order);
}
```

Note that every call to `Save` commits the transaction. There's no way to save two aggregates in one transaction. This is by design because Event Driven Architecture is the core of Taro (even thought Taro is just a Poor Man's implementation).

Events are published in the background, so you shouldn't assume events are all published when the `Save` method returns. Before being published, events will be saved in the same database where Orders are saved. They are also saved in the same transaction, so events won't be published if the `Save` method failed.

Events will be delivered 'at least once'. So it's possible that an event is published more than once. So you are required to apply 'Idempotency' to your event handlers.

## How it works ##

When calling `Save` method in domain repositories, the aggregate and the pending events are saved inside a db transaction, so events are guaranteed to be saved as long as the aggregate is saved successfully. Then a signal is sent to the `IRelayWorker`, which is responsible to 'relay' the events to the underlying transport. If the application crashed after the db commit but before the relay worker signaling. The relay worker will check all pending events in the database and publish them on the next startup.