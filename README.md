# Kassandra
The goal of this library is to provide the same interface to different kind of connectors (SQL only for the moment. MongoDB, Redis, MySQL, Oracle in progress)
## Usage
In the examples below I will illustrate how to use the framework in different situation
### Queries samples
``` C#
public class MonsterManager
{
	private readonly IContext _context;
	private readonly ILog _logger = LogManager.GetLogger<MonsterManager>();

	public MonsterManager(string connectionString)
	{
		_context = **SqlContextFactory.Instance.**GetContext(connectionString);
	}

	public IList<Monster> GetAll()
	{
		var cacheKey = string.Format("{0}-GetAll", GetType().Name);
		
		return _context
			.BuildQuery<Monster>("proc_Monster_GetAll")
			.Mapper(MonsterMapper)
			.UseCache(cacheKey, TimeSpan.FromHours(1))
			.ConnectionOpening(args => _logger.Trace("Connection will be opened"))
			.ConnectionOpened(args => _logger.Trace("Connection is opened"))
			.ConnectionClosing(args => _logger.Trace("Connection will be closed"))
			.ConnectionClosed(args => _logger.Trace("Connection is closed"))
			.QueryExecuting(args => _logger.Trace("Query will be executed"))
			.QueryExecuted(args => _logger.Trace("Query is executed"))
			.MustCatchExceptions()
			.QueryMany();
	}

	public Monster Get(int id)
	{
		var cacheKey = string.Format("{0}-Get-{1}", GetType().Name, id);
		
		return _context
			.BuildQuery<Monster>("pr_Monsters_GetByID")
			.Parameter("@MonsterID", id)
			.Mapper(MonsterMapper)
			.UseCache(cacheKey, TimeSpan.FromHours(1))
			.QuerySingle();
	}
	
	public void Insert(Monster monster)
	{
		_context.BuildQuery(@"INSERT INTO Monsters (Name, Power, Image) VALUES (@Name, @Power, @Image)", isCommand: false)
			.Parameter("@Name", monster.Name)
			.Parameter("@Power", monster.Power)
			.Parameter("@Image", monster.Image)
			.ExecuteNonQuery();
	}

	#region Mappers

	static MonsterManager()
	{
		MonsterMapper = new FunctionMapper<Monster>(reader =>
		{
			return new Monster
			{
				Id = reader.ValueAs<int>("ID"),
				Name = reader.ValueAs<string>("Name"),
				Power = reader.ValueAs<int>("Power"),
				ImageUrl = reader.ValueAs<string>("Image")
			};
		});
	}

	private static readonly IMapper<Monster> MonsterMapper;

	#endregion
}

```
