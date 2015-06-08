# Nuget
Core : https://www.nuget.org/packages/Kassandra.Core/

SQL Connector : https://www.nuget.org/packages/Kassandra.Connector.Sql/

# Kassandra
The goal of this library is to provide the same interface to different kind of connectors (SQL only for the moment. MySQL, Oracle in progress)
## Class explanations
### IContext
The abstration of the database. This interface extends IQueryBuilder so that an IContext instance can return an IQuery object.
#### IContextFactory
Each Connector package contains a factory. The namespace of the connectors factory is always defined as follow : Kassandra.Connector.*TYPE*.Factories. (eg: Kassandra.Connector.Sql.Factories.SqlContextFactory). Those factories are singletons factories so use the static approach to use them.
### Queries interfaces
#### IQuery
This interface is built using 'fluent' approach. 

**Parameter** method defines a parameter of the query item. 

A condition can be defined to check if the parameter has to be added or not. This case is useful when using parameters that has to be defined in some cases without embracing the IQuery instance in an id clause. So the aim of this is to keep the code as clean as possible using FluentApi approach.

**MustCatchExceptions** can be invoked. When used all exceptions throwed in the query execution path will be catched and no information will be available for tracing and debuging. 

To log the error throwed, use the *Error* method as used in the MonsterManager.GetAll example above.

**ConnectionOpening**, **ConnectionOpened**, **ConnectionClosing**, **ConnectionClosed**, **QueryExecuting**, **QueryExecuted** and **Error** methods are event handlers that can be defined to do a specific job at a specific moment. 

Note that the **Error** method is used in the *catch(Exception)* regardless the **MustCatchExceptions** invokation in the query building.

**ExecuteNonQuery** will send the query and will not return a result.

#### IResultQuery
This interface extends the IQuery interface and add methods used in an 'output' context.

**UseCache** will check in the cache if the provided cacheKey exists and use its content as direct result whitout calling the database behind the query. A cache duration can be defined using a *Timespan*.
If not value are defined, the cacheKey will be computed based on the command name
**Mapper** defines the mapper used when an item is cast from a **IResultReader** to an output type.

A mapper **must** be defined for an IResultQuery, otherwise an Exception is throwed and the query will fail.

**QueryMany**, **QuerySingle**, **QueryScalar** will send the query and return the result using the IMapper item defined from the **Mapper** command.

#### ITransaction
Call **Commit** to perform all queries enqued using **AddQuery**. If everything went ok, no exception are throwed. Otherwise, a Rollback must be performed.

The **ITransaction.Commit()** must be included in a try-catch clause with a rollback statement in the catch clause.

## Usage
The examples below illustrate how to use the framework in different situations
### Queries samples
``` C#
public class MonsterManager
{
	private readonly IContext _context;
	private readonly ILog _logger = LogManager.GetLogger<MonsterManager>();

	public MonsterManager(string connectionString)
	{
		_context = **Kassandra.Connector.Sql.Factories.SqlContextFactory.Instance.**GetContext(connectionString);
	}

	#region Get

	// Get without cache
	public IList<Monster> GetAll()
	{		
		return _context
			.BuildQuery<Monster>("proc_Monster_GetAll")
			.Mapper(new ExpressionMapper<Monster>(
                new MappingItem<Monster>(x => x.Name, "Name"),
                new MappingItem<Monster>(x => x.Power, "Strength"),
                new MappingItem<Monster>(x => x.Id, "ID"),
                new MappingItem<Monster>(x => x.Uid, "UID")
            )
			.QueryMany();
	}

	// Get with cache
	public IList<Monster> GetAllCache()
	{		
		return _context
			.BuildQuery<Monster>("proc_Monster_GetAll")
			.Mapper(new ExpressionMapper<Monster>(
                new MappingItem<Monster>(x => x.Name, "Name"),
                new MappingItem<Monster>(x => x.Power, "Strength"),
                new MappingItem<Monster>(x => x.Id, "ID"),
                new MappingItem<Monster>(x => x.Uid, "UID")
            )
			.UseCache()
			.QueryMany();
	}

	// Get with cachekey
	public IList<Monster> GetAllCacheKeyDefined()
	{		
		return _context
			.BuildQuery<Monster>("proc_Monster_GetAll")
			.Mapper(new ExpressionMapper<Monster>(
                new MappingItem<Monster>(x => x.Name, "Name"),
                new MappingItem<Monster>(x => x.Power, "Strength"),
                new MappingItem<Monster>(x => x.Id, "ID"),
                new MappingItem<Monster>(x => x.Uid, "UID")
            )
			.UseCache("Monster-GetAll")
			.QueryMany();
	}

	// Get with cachekey and duration
	public IList<Monster> GetAllCacheKeyDefined()
	{		
		return _context
			.BuildQuery<Monster>("proc_Monster_GetAll")
			.Mapper(new ExpressionMapper<Monster>(
                new MappingItem<Monster>(x => x.Name, "Name"),
                new MappingItem<Monster>(x => x.Power, "Strength"),
                new MappingItem<Monster>(x => x.Id, "ID"),
                new MappingItem<Monster>(x => x.Uid, "UID")
            )
			.UseCache("Monster-GetAll", TimeSpan.FromHours(2))
			.QueryMany();
	}

	// Get with duration
	public IList<Monster> GetAllCacheKeyDefined()
	{		
		return _context
			.BuildQuery<Monster>("proc_Monster_GetAll")
			.Mapper(new ExpressionMapper<Monster>(
                new MappingItem<Monster>(x => x.Name, "Name"),
                new MappingItem<Monster>(x => x.Power, "Strength"),
                new MappingItem<Monster>(x => x.Id, "ID"),
                new MappingItem<Monster>(x => x.Uid, "UID")
            )
			.UseCache(duration: TimeSpan.FromHours(2))
			.QueryMany();
	}

	#endregion
	
	public void Insert(Monster monster)
	{
		_context.BuildQuery(@"INSERT INTO Monsters (Name, Power, Image) VALUES (@Name, @Power, @Image)", isCommand: false)
			.Parameter("@Name", monster.Name)
			.Parameter("@Power", monster.Power)
			.Parameter("@Image", monster.Image)
			.ExecuteNonQuery();
	}

	public int InsertAndGetId(Monster monster)
	{
		return _context.BuildQuery<int>(@"INSERT INTO Monsters (Name, Power, Image) OUTPUT Inserted.ID VALUES (@Name, @Power, @Image)", isCommand: false)
			.Parameter("@Name", monster.Name)
			.Parameter("@Power", monster.Power)
			.Parameter("@Image", monster.Image)
			.QueryScalar();
	}

	#region Insert using transactions

	public void DestroyCity(int cityId){
		ITransaction transaction = _context.BuildTransaction("monster_destroy_city");

        IQuery removeMonstersFromCity = _context.BuildQuery("DELETE FROM MonstersCity WHERE CityID = @CityID")
            .Parameter("@CityID", cityId);
        IQuery removeCity = _context.BuildQuery("DELETE FROM Cities WHERE ID = @CityID")
            .Parameter("@CityID", cityId);

        transaction.AddQuery(removeMonstersFromCity);
        transaction.AddQuery(removeCity);

        try
        {
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
	}

	#endregion
}

```
