namespace Kassandra.Core
{
    public interface IQueryBuilder
    {
        IResultQuery<TOutput> BuildQuery<TOutput>(string query, bool isCommand = true);
        IQuery BuildQuery(string query, bool isCommand = true);
    }
}