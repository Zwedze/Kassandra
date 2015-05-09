using Common.Logging;
using Kassandra.Core.Interfaces;

namespace Kassandra.Core
{
    public abstract class BaseDatabase : IDataContext
    {
        protected readonly ILog Logger;

        protected BaseDatabase()
        {
            Logger = LogManager.GetLogger<IDataContext>();
        }

        public abstract ITransaction BuildTransaction(string transactionName);
        //public abstract IConnection BuildConnection(ConnectionAccess connectionAccess);
        public abstract IResultQuery<TOutput> BuildQuery<TOutput>(string query, bool isCommand = true);
        public abstract IQuery BuildQuery(string query, bool isCommand = true);
    }
}