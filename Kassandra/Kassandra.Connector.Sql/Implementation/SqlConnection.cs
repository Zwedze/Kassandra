using System.Data;
using Kassandra.Core.Interfaces;

namespace Kassandra.Connector.Sql.Implementation
{
    public class SqlConnection : IConnection, IDbConnection
    {
        private readonly IDbConnection _dbConnection;

        public SqlConnection(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            Name = dbConnection.Database;
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        public bool KeepOpen { get; set; }
        public string Name { get; set; }

        public IDbTransaction BeginTransaction()
        {
            return _dbConnection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _dbConnection.BeginTransaction(il);
        }

        public void Close()
        {
            _dbConnection.Close();
        }

        public void ChangeDatabase(string databaseName)
        {
            _dbConnection.ChangeDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            return _dbConnection.CreateCommand();
        }

        public void Open()
        {
            _dbConnection.Open();
        }

        public string ConnectionString
        {
            get { return _dbConnection.ConnectionString; }
            set { _dbConnection.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return _dbConnection.ConnectionTimeout; }
        }

        public string Database
        {
            get { return _dbConnection.Database; }
        }

        public ConnectionState State
        {
            get { return _dbConnection.State; }
        }
    }
}