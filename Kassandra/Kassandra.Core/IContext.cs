namespace Kassandra.Core
{
    public interface IContext : IQueryBuilder
    {
        ITransaction BuildTransaction(string transactionName);
        void ClearCache();
    }
}