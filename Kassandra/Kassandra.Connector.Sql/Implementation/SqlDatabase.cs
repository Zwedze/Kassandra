using Kassandra.Core;
using Kassandra.Core.Interfaces;

namespace Kassandra.Connector.Sql.Implementation
{
    internal class SqlDatabase : BaseDatabase
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly string _connectionString;

        public SqlDatabase(string connectionString, ICacheRepository cacheRepository)
        {
            _connectionString = connectionString;
            _cacheRepository = cacheRepository;
        }

        public override ITransaction BuildTransaction(string transactionName)
        {
            return new SqlTransaction(GetNewConnection(), transactionName, _cacheRepository);
        }

        public override IResultQuery<TOutput> BuildQuery<TOutput>(string query, bool isCommand = true)
        {
            return new SqlResultQuery<TOutput>(GetNewConnection(), query, isCommand, _cacheRepository);
        }

        public override IQuery BuildQuery(string query, bool isCommand = true)
        {
            return new SqlQuery(GetNewConnection(), query, isCommand);
        }

        protected SqlConnection GetNewConnection()
        {
            return new SqlConnection(new System.Data.SqlClient.SqlConnection(_connectionString));
        }
    }
}