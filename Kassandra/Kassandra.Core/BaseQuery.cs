using System;
using System.Text;
using Common.Logging;
using Kassandra.Core.Interfaces;
using Kassandra.Core.Models.Query;

namespace Kassandra.Core
{
    public abstract class BaseQuery : IQuery
    {
        protected BaseQuery(string commandName)
        {
            CommandeName = commandName;
            OnError += GenericError;
            OnConnectionOpening += GenericConnectionOpening;
            OnConnectionOpened += GenericConnectionOpened;
            OnQueryExecuting += GenericQueryExecuting;
            OnQueryExecuted += GenericQueryExecuted;
            OnConnectionClosing += GenericConnectionClosing;
            OnConnectionClosed += GenericConnectionClosed;
        }

        public bool CatchExceptions { get; set; }
        public string CommandeName { get; set; }
        //public abstract ITransaction Transaction { get; set; }
        public abstract ILog Logger { get; protected set; }
        public abstract IQuery Parameter(string parameterName, object parameterValue);
        public abstract void ExecuteNonQuery();

        #region Events

        #region Error

        public event Action<QueryErrorEventArgs> OnError;

        protected void OnErrorHandler(QueryErrorEventArgs args)
        {
            var handler = OnError;
            if (handler != null)
            {
                handler(args);
            }
        }

        private void GenericError(QueryErrorEventArgs args)
        {
            var message = new StringBuilder(args.Exception.Message);
            if (!string.IsNullOrWhiteSpace(args.AdditionalMessage))
            {
                message.Append(args.AdditionalMessage);
            }
            Logger.Error(message);
        }

        public IQuery Error(Action<QueryErrorEventArgs> action)
        {
            OnError += action;
            return this;
        }

        #endregion

        #region ConnectionOpening

        public event Action<OpenConnectionEventArgs> OnConnectionOpening;

        protected void OnConnectionOpeningHandler(OpenConnectionEventArgs args)
        {
            var handler = OnConnectionOpening;
            if (handler != null)
            {
                handler(args);
            }
        }

        private void GenericConnectionOpening(OpenConnectionEventArgs args)
        {
            Logger.Trace(string.Format("Opening connection {0}", args.Connection.Name));
        }

        public IQuery ConnectionOpening(Action<OpenConnectionEventArgs> action)
        {
            OnConnectionOpening += action;
            return this;
        }

        #endregion

        #region ConnectionOpened

        public event Action<OpenConnectionEventArgs> OnConnectionOpened;

        protected void OnConnectionOpenedHandler(OpenConnectionEventArgs args)
        {
            var handler = OnConnectionOpened;
            if (handler != null)
            {
                handler(args);
            }
        }

        private void GenericConnectionOpened(OpenConnectionEventArgs args)
        {
            Logger.Trace(string.Format("Opened connection {0}", args.Connection.Name));
        }

        public IQuery ConnectionOpened(Action<OpenConnectionEventArgs> action)
        {
            OnConnectionOpened += action;
            return this;
        }

        #endregion

        #region QueryExecuting

        public event Action<QueryExecutionEventArgs> OnQueryExecuting;

        protected void OnQueryExecutingHandler(QueryExecutionEventArgs args)
        {
            var handler = OnQueryExecuting;
            if (handler != null)
            {
                handler(args);
            }
        }

        public IQuery QueryExecuting(Action<QueryExecutionEventArgs> action)
        {
            OnQueryExecuting += action;
            return this;
        }

        private void GenericQueryExecuting(QueryExecutionEventArgs args)
        {
            Logger.Trace(string.Format("Executing query {0}", args.Query.CommandeName));
        }

        #endregion

        #region QueryExecuted

        public event Action<QueryExecutionEventArgs> OnQueryExecuted;

        protected void OnQueryExecutedHandler(QueryExecutionEventArgs args)
        {
            var handler = OnQueryExecuted;
            if (handler != null)
            {
                handler(args);
            }
        }

        private void GenericQueryExecuted(QueryExecutionEventArgs args)
        {
            Logger.Trace(string.Format("Executed query {0}", args.Query.CommandeName));
        }

        public IQuery QueryExecuted(Action<QueryExecutionEventArgs> action)
        {
            OnQueryExecuted += action;

            return this;
        }

        #endregion

        #region ConnectionClosing

        public event Action<CloseConnectionEventArgs> OnConnectionClosing;

        protected void OnConnectionClosingHandler(CloseConnectionEventArgs args)
        {
            var handler = OnConnectionClosing;
            if (handler != null)
            {
                handler(args);
            }
        }

        private void GenericConnectionClosing(CloseConnectionEventArgs args)
        {
            Logger.Trace(string.Format("Closing connection {0}", args.Connection.Name));
        }

        public IQuery ConnectionClosing(Action<CloseConnectionEventArgs> action)
        {
            OnConnectionClosing += action;

            return this;
        }

        #endregion

        #region ConnectionClosed

        public event Action<CloseConnectionEventArgs> OnConnectionClosed;

        protected void OnConnectionClosedHandler(CloseConnectionEventArgs args)
        {
            var handler = OnConnectionClosed;
            if (handler != null)
            {
                handler(args);
            }
        }

        private void GenericConnectionClosed(CloseConnectionEventArgs args)
        {
            Logger.Trace(string.Format("Closed connection {0}", args.Connection.Name));
        }

        public IQuery ConnectionClosed(Action<CloseConnectionEventArgs> action)
        {
            OnConnectionClosed += action;

            return this;
        }

        #endregion

        #endregion
    }
}