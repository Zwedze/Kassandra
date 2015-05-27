using System;
using System.Collections.Generic;
using Kassandra.Core.Models.Query;

namespace Kassandra.Core
{
    public interface IResultQuery<TOutput> : IQuery
    {
        IResultQuery<TOutput> UseCache(string cacheKey = null, TimeSpan? duration = null);
        IResultQuery<TOutput> Mapper(IMapper<TOutput> mapper);
        new IResultQuery<TOutput> Parameter(string parameterName, object parameterValue);
        new IResultQuery<TOutput> Error(Action<QueryErrorEventArgs> args);
        new IResultQuery<TOutput> ConnectionOpening(Action<OpenConnectionEventArgs> args);
        new IResultQuery<TOutput> ConnectionOpened(Action<OpenConnectionEventArgs> args);
        new IResultQuery<TOutput> QueryExecuting(Action<QueryExecutionEventArgs> args);
        new IResultQuery<TOutput> QueryExecuted(Action<QueryExecutionEventArgs> args);
        new IResultQuery<TOutput> ConnectionClosing(Action<CloseConnectionEventArgs> args);
        new IResultQuery<TOutput> ConnectionClosed(Action<CloseConnectionEventArgs> args);
        new IResultQuery<TOutput> MustCatchExceptions();
        IList<TOutput> QueryMany();
        TOutput QuerySingle();
        TOutput QueryScalar();
    }
}