﻿using System;
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
        }

        public IDbCommand Command { get; private set; }
        public override sealed ILog Logger { get; protected set; }

        public override IQuery Parameter(string parameterName, object parameterValue)
        {
            var parameter = Command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            Command.Parameters.Add(parameter);

            return this;
        }

        public override void ExecuteNonQuery()
        {
            OpenConnection();
            try
            {
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
                CloseConnection();
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