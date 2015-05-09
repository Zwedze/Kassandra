using System;
using Kassandra.Core.Models.Query;

namespace Kassandra.Core
{
    public interface IQuery
    {
        string Query { get; set; }
        IQuery MustCatchExceptions();
        IQuery Parameter(string parameterName, object parameterValue);
        void ExecuteNonQuery();
        IQuery Error(Action<QueryErrorEventArgs> args);
        IQuery ConnectionOpening(Action<OpenConnectionEventArgs> args);
        IQuery ConnectionOpened(Action<OpenConnectionEventArgs> args);
        IQuery QueryExecuting(Action<QueryExecutionEventArgs> args);
        IQuery QueryExecuted(Action<QueryExecutionEventArgs> args);
        IQuery ConnectionClosing(Action<CloseConnectionEventArgs> args);
        IQuery ConnectionClosed(Action<CloseConnectionEventArgs> args);
    }

    public enum QueryType
    {
        Query,
        StoredProcedure
    }
}