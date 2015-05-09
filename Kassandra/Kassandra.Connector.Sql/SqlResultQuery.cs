﻿using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Kassandra.Core;
using Kassandra.Core.Models.Query;

namespace Kassandra.Connector.Sql
{
    internal class SqlResultQuery<TOutput> : SqlQuery, IResultQuery<TOutput>
    {
        private readonly ICacheRepository _cacheRepository;
        private TimeSpan _cacheDuration;
        private string _cacheKey;
        private IMapper<TOutput> _mapper;

        public SqlResultQuery(SqlConnection connection, string query, bool isStoredProcedure,
            ICacheRepository cacheRepository)
            : base(connection, query, isStoredProcedure)
        {
            // _mapper = new StubMapper<TOutput>();
            _cacheKey = null;
            _cacheDuration = default(TimeSpan);
            _cacheRepository = cacheRepository;
        }

        public new IResultQuery<TOutput> MustCatchExceptions()
        {
            CatchExceptions = true;
            return this;
        }

        public IResultQuery<TOutput> UseCache(string cacheKey, TimeSpan duration)
        {
            _cacheKey = cacheKey;
            _cacheDuration = duration;

            return this;
        }

        public IResultQuery<TOutput> Mapper(IMapper<TOutput> mapper)
        {
            _mapper = mapper;
            return this;
        }

        public new IResultQuery<TOutput> Parameter(string parameterName, object parameterValue)
        {
            base.Parameter(parameterName, parameterValue);

            return this;
        }

        public IList<TOutput> QueryMany()
        {
            if (_mapper == null)
            {
                throw new ArgumentException("No IMapper defined");
            }

            if (!string.IsNullOrWhiteSpace(_cacheKey))
            {
                var output = _cacheRepository.GetEntry<IList<TOutput>>(_cacheKey);
                if (output != null)
                {
                    return output;
                }
            }

            OpenConnection();
            try
            {
                OnQueryExecutingHandler(new QueryExecutionEventArgs {Query = this});
                var reader = Command.ExecuteReader();
                OnQueryExecutedHandler(new QueryExecutionEventArgs {Query = this});
                var output = _mapper.MapToList(new SqlDataReader(reader));

                if (!string.IsNullOrWhiteSpace(_cacheKey))
                {
                    _cacheRepository.Insert(_cacheKey, output, _cacheDuration, CacheItemPriority.Default);
                }

                return output;
            }
            catch (Exception e)
            {
                OnErrorHandler(new QueryErrorEventArgs {Exception = e});
                if (CatchExceptions)
                {
                    return default(IList<TOutput>);
                }
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public TOutput QuerySingle()
        {
            if (_mapper == null)
            {
                throw new ArgumentException("No IMapper defined");
            }

            OpenConnection();
            try
            {
                OnQueryExecutingHandler(new QueryExecutionEventArgs {Query = this});
                var reader = Command.ExecuteReader();
                OnQueryExecutedHandler(new QueryExecutionEventArgs {Query = this});
                return _mapper.Map(new SqlDataReader(reader));
            }
            catch (Exception e)
            {
                OnErrorHandler(new QueryErrorEventArgs {Exception = e});
                if (CatchExceptions)
                {
                    return default(TOutput);
                }
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public TOutput QueryScalar()
        {
            OpenConnection();
            try
            {
                OnQueryExecutingHandler(new QueryExecutionEventArgs {Query = this});
                var ret = (TOutput) Command.ExecuteScalar();
                OnQueryExecutedHandler(new QueryExecutionEventArgs {Query = this});
                return ret;
            }
            catch (Exception e)
            {
                OnErrorHandler(new QueryErrorEventArgs {Exception = e});
                if (CatchExceptions)
                {
                    return default(TOutput);
                }
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        #region Events

        public new IResultQuery<TOutput> Error(Action<QueryErrorEventArgs> args)
        {
            base.Error(args);

            return this;
        }

        public new IResultQuery<TOutput> ConnectionOpening(Action<OpenConnectionEventArgs> args)
        {
            base.ConnectionOpening(args);

            return this;
        }

        public new IResultQuery<TOutput> ConnectionOpened(Action<OpenConnectionEventArgs> args)
        {
            base.ConnectionOpened(args);

            return this;
        }

        public new IResultQuery<TOutput> QueryExecuting(Action<QueryExecutionEventArgs> args)
        {
            base.QueryExecuting(args);

            return this;
        }

        public new IResultQuery<TOutput> QueryExecuted(Action<QueryExecutionEventArgs> args)
        {
            base.QueryExecuted(args);

            return this;
        }

        public new IResultQuery<TOutput> ConnectionClosing(Action<CloseConnectionEventArgs> args)
        {
            base.ConnectionClosing(args);

            return this;
        }

        public new IResultQuery<TOutput> ConnectionClosed(Action<CloseConnectionEventArgs> args)
        {
            base.ConnectionClosed(args);

            return this;
        }

        #endregion
    }
}