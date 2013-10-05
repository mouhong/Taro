# Taro Framework

### Bring domain events to your application

## How to use

### 1. Implement a unit of work

Add a unit of work implementation specific to your data access component (inherits from UnitOfWorkBase).
For example, for Linq to SQL, you can add this one:

```csharp
public class UnitOfWork : UnitOfWorkBase
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
public class MoneyTransfered : Taro.DomainEvent
{
	public Account SourceAccount { get; private set; }

    public Account DestinationAccount { get; private set; }
 
    public decimal Amount { get; private set; }

	// ...
}
```

### 5. Raise domain events

Domain events should be located in the domain model. So it can only be fired from entities or domain services. For example:

```csharp
public class MoneyTransferService 
{
	public void Transfer(Account source, Account dest, decimal amount) 
	{
		// business logic goes here
		
		// Raise OrderDelivered event
		DomainEvent.Apply(new MoneyTransfered(...));
	}
}
```

### 6. Handle domain events

Event handlers should implement `IHandle<TEvent>` interface, like this:

```csharp
[AwaitComitted, HandleAsync]
public class OnMoneyTransfered_NotifyOwner : IHandle<MoneyTransfered>
{
	public void Handle(MoneyTransfered evnt) 
	{
		// Mail to account owner: Money transfer succeeded
	}
}
```

**AwaitCommitted attribute:**

Means this handler will not be executed immediately after the event is fired. It'll wait unit the unit of work is committed successfully.
If your handler code need to be transactional, simply remove this attribute. Then it'll be executed before the unit of work is committed.

But please note that, if you remove the `AwaitCommitted` attribute, it often means that you also need to remove the `HandleAsync` attribute.

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
	var account1 = unitOfWork.Query<Account>()
						     .FirstOrDefault(x => x.Id == ...);
    var account2 = unitOfWork.Query<Account>()
 							 .FirstOrDefault(x => x.Id == ...);

	var service = new MoneyTransferService();
	service.Transfer(account1, account2, 1024);

	// Calling Complete will commit the unit of work
	scope.Complete();
}
```

If you want to introduce an additional application service layer, you need to move above code to your application service layer.

Happy coding!

## Resources

Email: dylan.lin [AT] live.com

Weibo: http://t.qq.com/mouhong-lin
