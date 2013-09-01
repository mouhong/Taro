# Taro Framework

### Bring domain events to your application

## How to use

### 1. Implement a unit of work

Add an implementation of `IUnitOfWork` specifc to your data access component.
For example, for Linq to SQL, you can add this one:

```csharp
public class UnitOfWork : AbstractUnitOfWork 
{
	private DataContext _dataContext;

	public UnitOfWork(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public void Query<T>() where T : class
	{
		return _dataContext.GetTable<T>();
	}

	public void Add<T>(T entity) where T : class 
	{
		_dataContext.GetTable<T>().InsertOnSubmit(entity);
	}

	public void Delete<T>(T entity) where T : class
	{
		_dataContext.GetTable<T>().DeleteOnSubmit(entity);
	}

	protected override void DoCommit()
	{
		_dataContext.SubmitChanges();
	}
}
```

### 2. Implement a unit of work scope

```csharp
public class UnitOfWorkScope : UnitOfWorkScope<UnitOfWork>
{
}
```

It's simply setting the generic parameter of `UnitOfWorkScope<TUnitOfWork>` to your `IUnitOfWork` implementation. This is not actually required.

### 3. Implement your domain model

Implement domain models in your project

### 4. Add domain events

Domain events should inherit from `Taro.DomainEvent`, like this:

```csharp
public class OrderDelivered : Taro.DomainEvent
{
	public Order Order { get; private set; }

	public OrderDelivered(Order order)
	{
		Order = order;
	}
}
```

### 5. Raise domain events

Domain events should be located in the domain model. So it can only be fired from entities or domain services. For example:

```chsarp
// A domain service to handle order delivery
public class DeiveryService 
{
	public void DeliverOrder(Order order) 
	{
		// business logic goes here
		
		// Raise OrderDelivered event
		DomainEvent.Apply(new OrderDelivered(order));
	}
}
```

### 6. Handle domain events

Event handlers should implement `IHandle<TEvent>` interface, like this:

```csharp
[AwaitComitted, HandleAsync]
public class OnOrderDelivered_NotifyCustomer : IHandle<OrderDelivered>
{
	public void Handle(OrderDelivered evnt) 
	{
		// Mail to customer: Your order was delivered
	}
}
```

**AwaitCommitted attribute:**

Means this handler will not be executed immediately after the event is fired. It'll wait unit the unit of work is committed successfully.
If your handler code need to be transactional, simply remove this attribute. Then it'll be executed before the unit of work is committed.

But please note that, if you remove the `AwaitCommitted` attribute, it often means that you need to also remove the `HandleAsync` attribute.

**HandleAsync attribute:**

Means this handler will be executed in async manner.

### 7. Taro Configuration

You need to configure Taro in you application entry point (For example, in asp.net, it's `Global.asax`), like this:

```csharp
Taro.Config.TaroEnvironment.Configure(taro => 
{
	taro.UsingUnitOfWorkFactory(() => new UnitOfWork(...));
	taro.UsingDefaultEventDispatcher(Assembly.Load("YourEventHandlersAssembly"));
});
```

### 8. Wire up

In you controller action (in Webform, it might be in your `.aspx.cs`), write this down:

```csharp
using (var scope = new UnitOfWorkScope()) 
{
	var unitOfWork = scope.UnitOfWork;
	var order = unitOfWork.Query<Order>()
						  .FirstOrDefault(o => o.Id = xxx);

	var deliveryService = new DeliveryService();
	deliveryService.DeliverOrder(order);

	// Calling Complete will commit the unit of work
	scope.Complete();
}
```

If you want to introduce an additional application service layer, you need to move above code to your application service layer.

Happy coding!

## Resources

Email: dylan.lin [AT] live.com

Weibo: http://t.qq.com/mouhong-lin
