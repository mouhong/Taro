# Taro Framework

### Bring domain events to your application

## What it is

Event-driven architecture is an attractive concept. Many existing frameworks have already been built base on this concept. But I think they are too heavy weight for small applications. Here "small applications" refers to the applications which have rather complex business logic but do not have heavy load (so they are often hosted in only one or two servers).

To embrace the event-driven architecture without using heavy weight frameworks. I used to create a simple alternative like this:

```csharp
// The event handler
public class OrderApprovedNotifier : IHandles<OrderApproved>
{
    public void Handle(OrderApproved theEvent)
    {
        // Send an email to customer
    }
}

// The order
public class Order
{
    public void Approve()
    {
		// This will publish events immediately, causing event handlers to run
        Event.Publish(new OrderApproved 
		{
    		OrderId = this.Id
		});
    }
}

// The mvc controller

var order = unitOfWork.Get<Order>(orderId);
// This will cause event handlers to run immediately
order.Approve();
// This may fail
unitOfWork.Save(order);

```

This solution is problematic. When calling `order.Approve()`, the `OrderApprovedNotifier` will be called immediately, then an email will be send to the customer. This is done before calling `unitOfWork.Save(order)`!! What if `unitOfWork.Save(order)` fails? The customer receives an email saying the order is approved while the approve operation is actually failed. So we need to delay the event handlers until the order is successfully saved. So we can come up with a better one:

```csharp
public class Order : AggregateRoot
{
    public void Approve()
    {
		// Append the event to a queue instead of publishing it immediately
  		Event.Append(new OrderApproved
		{
			OrderId = this.Id
		});
    }
}

public class UnitOfWork
{
    public void Save(AggregateRoot root)
	{
		// Save order to database first
        db.Save(root);

		// If the order is successfully saved, 
		// publish the events associated with it.
		foreach (var eachEvent in root.GetEvents())
		{
			Event.Publish(eachEvent);
		}
	}
}

```

This time we ensure the events are published after the order is successfully saved. If `unitOfWork.Save(order)` fails, events will not be published, and the customer won't get the confusing email. That's good. 

However, event publishing may fail. The order may be successfully saved, but the events may not be successfully published. Failing to get the notification email is actually acceptable, but what if the event handler is executing some important business logic? So we need to ensure that, if the `order` is successfully saved, all events should be published (at least once).

2PC is a solution to this problem. But it's expensive, and it's not supported by all data stores. So I come up with another solution:

- Temporarily save events in the same data store where domain objects are saved in a local transaction
- Publish events in background when the transaction is successfully committed
- A signal will be send to the background worker when events are saved, so event handlers can be invoked near realtime

Because events and domain objects are saved in the same data store, 2PC can be avoid. And because they are saved in one transaction, it's impossible to lose events.

Taro is an event driven framework built with the last solution. I see Taro as a Poor Man's framework because it's targeting small applications.

## How to use

### 1. Configure Taro in app start ##

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

### 2. Define domain events ###

```csharp
public class OrderApproved : Event
{
    public int OrderId { get; set; }
}
```

Domain events are POCOs inheriting from `Taro.Event`.

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

Aggregate roots are domain objects inheriting from `Taro.AggregateRoot`. Only aggregate roots are allowed to append domain events (using the `AppendEvent` method). I use `AppendEvent` instead of `PublishEvent` to emphasize that the event is queued instead of being published immediately.

### 4. Handle events ###

```csharp
public class OrderApprovedNotifier : IHandles<OrderApproved>
{
    public void Handle(OrderApproved theEvent)
    {
        // Send email to customer...
    }
}
```

### 5. Save aggregates ###

Taro does not have an abstraction on data access. If you are using the Taro.RavenDB package, you should use `Taro.IRavenDomainRepository` to save aggregates. This is because you might want to use RavenDB specific data access methods which are difficult to abstract. Following code illustrates how to save the `Order` aggregate:

```csharp
using (var repository = AppRuntime.Instance.CreateDomainRepository<IRavenDomainRepository>())
{
    var order = repository.Find<Order>("orders/1");
    order.Approve();

    repository.Save(order);
}
```

Note that every call to `Save` commits the transaction. There's no way to save two aggregates in one transaction. This is by design because Taro is designed to be an event-driven framework (event thought it's just a poor man's implementation).

Events are published in the background, so you shouldn't assume events are all published when the `Save` method returns.

Events will be delivered 'at least once'. It's possible that an event is published more than once. So you are required to apply 'Idempotency' to your event handlers.