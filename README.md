# Kassandra
The goal of this library is to provide the same interface to different kind of connectors (SQL only for the moment. MongoDB, Redis, MySQL, Oracle in progress)
## Usage
In the examples below I will illustrate how to use the framework using the abstract classes
### Text queries
``` C#
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
```
