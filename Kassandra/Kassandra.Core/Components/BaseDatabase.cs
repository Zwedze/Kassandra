using Common.Logging;

namespace Kassandra.Core.Components
{
    public abstract class BaseDatabase : IContext
    {
        protected readonly ILog Logger;

        protected BaseDatabase()
        {
            Logger = LogManager.GetLogger<IContext>();
        }

        public abstract ITransaction BuildTransaction(string transactionName);
        public void ClearCache()
        {
            System.Runtime.Caching.MemoryCache.Default.Trim(100);
        }

        //public abstract IConnection BuildConnection(ConnectionAccess connectionAccess);
        public abstract IResultQuery<TOutput> BuildQuery<TOutput>(string query, bool isCommand = true);
        public abstract IQuery BuildQuery(string query, bool isCommand = true);
    }
}