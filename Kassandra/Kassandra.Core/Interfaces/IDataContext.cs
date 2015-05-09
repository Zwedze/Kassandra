namespace Kassandra.Core.Interfaces
{
    public interface IDataContext : IQueryBuilder
    {
        ITransaction BuildTransaction(string transactionName);
    }
}