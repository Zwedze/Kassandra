using System;
using System.Collections.Generic;
using System.Data;
using Common.Logging;
using Kassandra.Core;
using Kassandra.Core.Components;
using Kassandra.Core.Models.Query;

namespace Kassandra.Connector.Sql
{
    internal class SqlQuery : BaseQuery
    {
        private readonly SqlConnection _connection;

        public SqlQuery(SqlConnection connection, string query, bool isStoredProcedure) : base(query)
        {
            _connection = connection;
            Logger = LogManager.GetLogger<IQuery>();

            Command = connection.CreateCommand();
            Command.CommandText = query;
            if (isStoredProcedure)
            {
                Command.CommandType = CommandType.StoredProcedure;
            }

            Parameters = new Dictionary<string, object>();
        }

        protected IDictionary<string, object> Parameters { get; }
        public IDbCommand Command { get; }
        public override sealed ILog Logger { get; protected set; }

        public override IQuery Parameter(string parameterName, object parameterValue, bool condition = true)
        {
            if (!condition)
            {
                return this;
            }

            IDbDataParameter parameter = Command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue ?? DBNull.Value;
            Command.Parameters.Add(parameter);

            Parameters.Add(parameterName, parameterValue);

            return this;
        }

        public override void ExecuteNonQuery()
        {
            try
            {
                OpenConnection();
                OnQueryExecutingHandler(new QueryExecutionEventArgs {Query = this});
                Command.ExecuteNonQuery();
                OnQueryExecutedHandler(new QueryExecutionEventArgs {Query = this});
            }
            catch (Exception e)
            {
                OnErrorHandler(new QueryErrorEventArgs {Exception = e});

                if (CatchExceptions)
                {
                    return;
                }
                throw;
            }
            finally
            {
                try
                {
                    CloseConnection();
                }
                catch (Exception e)
                {
                    OnErrorHandler(new QueryErrorEventArgs {Exception = e});

                    if (!CatchExceptions)
                    {
                        throw;
                    }
                }
            }
        }

        protected void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                OnConnectionOpeningHandler(new OpenConnectionEventArgs {Connection = _connection});
                _connection.Open();
                OnConnectionOpenedHandler(new OpenConnectionEventArgs {Connection = _connection});
            }
        }

        protected void CloseConnection()
        {
            if (!_connection.KeepOpen)
            {
                OnConnectionClosingHandler(new CloseConnectionEventArgs {Connection = _connection});
                _connection.Close();
                OnConnectionClosedHandler(new CloseConnectionEventArgs {Connection = _connection});
            }
        }
    }
}