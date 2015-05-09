using System.Data;
using Common.Logging;
using Kassandra.Core.Interfaces;

namespace Kassandra.Connector.Sql.Implementation
{
    internal class SqlTransaction : ITransaction, IDbTransaction
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly SqlConnection _connection;
        private readonly ILog _logger;
        private readonly IDbTransaction _transaction;
        private readonly string _transactionName;

        public SqlTransaction(SqlConnection connection, string transactionName, ICacheRepository cacheRepository)
        {
            _connection = connection;
            _transactionName = transactionName;
            _cacheRepository = cacheRepository;
            if (connection.State != ConnectionState.Open)
                connection.Open();
            _connection.KeepOpen = true;
            _transaction = connection.BeginTransaction();
            _logger = LogManager.GetLogger<ITransaction>();
        }

        public void Dispose()
        {
            _transaction.Dispose();
            _connection.Dispose();
        }

        public void AddQuery(IQuery query)
        {
            if (!(query is SqlQuery))
            {
                return;
            }
            (query as SqlQuery).Command.Transaction = _transaction;
            query.ExecuteNonQuery();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _logger.Error(string.Format("Rollback: transaction #{0}", _transactionName));
            _transaction.Rollback();
        }

        public IResultQuery<TOutput> BuildQuery<TOutput>(string query, bool isCommand = true)
        {
            return new SqlResultQuery<TOutput>(_connection, query, isCommand, _cacheRepository);
        }

        public IQuery BuildQuery(string query, bool isCommand = true)
        {
            return new SqlQuery(_connection, query, isCommand);
        }

        #region IDbTransation

        public IDbConnection Connection
        {
            get { return _transaction.Connection; }
        }

        public IsolationLevel IsolationLevel
        {
            get { return _transaction.IsolationLevel; }
        }

        #endregion
    }
}