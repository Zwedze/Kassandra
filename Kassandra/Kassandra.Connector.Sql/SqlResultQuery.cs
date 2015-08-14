using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Kassandra.Core;
using Kassandra.Core.Models.Query;

namespace Kassandra.Connector.Sql
{
    internal class SqlResultQuery<TOutput> : SqlQuery, IResultQuery<TOutput>
    {
        private readonly ICacheRepository _cacheRepository;
        private TimeSpan? _cacheDuration;
        private string _cacheKey;
        private IMapper<TOutput> _mapper;
        private bool _useCache;

        public SqlResultQuery(SqlConnection connection, string query, bool isStoredProcedure,
            ICacheRepository cacheRepository)
            : base(connection, query, isStoredProcedure)
        {
            _cacheRepository = cacheRepository;
            _useCache = false;
        }

        public new IResultQuery<TOutput> MustCatchExceptions()
        {
            CatchExceptions = true;
            return this;
        }

        public IResultQuery<TOutput> UseCache(string cacheKey = null, TimeSpan? duration = null)
        {
            _useCache = true;
            _cacheKey = cacheKey;
            _cacheDuration = duration;

            return this;
        }

        public IResultQuery<TOutput> Mapper(IMapper<TOutput> mapper)
        {
            _mapper = mapper;
            return this;
        }

        public new IResultQuery<TOutput> Parameter(string parameterName, object parameterValue, bool condition = true)
        {
            base.Parameter(parameterName, parameterValue, condition);

            return this;
        }

        public IList<TOutput> QueryMany()
        {
            if (_mapper == null)
            {
                throw new ArgumentException("No IMapper defined");
            }

            if (_useCache)
            {
                IList<TOutput> output = _cacheRepository.GetEntry<IList<TOutput>>(GetCacheKey());
                if (output != null)
                {
                    return output;
                }
            }

            OpenConnection();
            try
            {
                OnQueryExecutingHandler(new QueryExecutionEventArgs {Query = this});
                IDataReader reader = Command.ExecuteReader();
                OnQueryExecutedHandler(new QueryExecutionEventArgs {Query = this});
                OnMappingExecutingHandler();
                IList<TOutput> output = _mapper.MapToList(new SqlDataReader(reader));
                OnMappingExecutedHandler();

                if (_useCache)
                {
                    _cacheRepository.Insert(GetCacheKey(), output, _cacheDuration ?? TimeSpan.FromHours(1));
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

            if (_useCache)
            {
                TOutput output = _cacheRepository.GetEntry<TOutput>(GetCacheKey());
                if (output != null && !output.Equals(default(TOutput)))
                {
                    return output;
                }
            }

            OpenConnection();
            try
            {
                OnQueryExecutingHandler(new QueryExecutionEventArgs {Query = this});
                IDataReader reader = Command.ExecuteReader();
                OnQueryExecutedHandler(new QueryExecutionEventArgs {Query = this});
                OnMappingExecutingHandler();
                TOutput output = _mapper.Map(new SqlDataReader(reader));
                OnMappingExecutedHandler();

                if (_useCache)
                {
                    _cacheRepository.Insert(GetCacheKey(), output, _cacheDuration ?? TimeSpan.FromHours(1));
                }

                return output;
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
            if (_useCache)
            {
                TOutput output = _cacheRepository.GetEntry<TOutput>(GetCacheKey());
                if (output != null && !output.Equals(default(TOutput)))
                {
                    return output;
                }
            }

            OpenConnection();
            try
            {
                OnQueryExecutingHandler(new QueryExecutionEventArgs {Query = this});
                TOutput output = (TOutput) Command.ExecuteScalar();
                OnQueryExecutedHandler(new QueryExecutionEventArgs {Query = this});
                if (_useCache)
                {
                    _cacheRepository.Insert(GetCacheKey(), output, _cacheDuration ?? TimeSpan.FromHours(1));
                }

                return output;
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

        private string GetCacheKey()
        {
            string cacheKey = _cacheKey;
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                StringBuilder cacheKeyBuilder = new StringBuilder(Query.ToLower());
                foreach (KeyValuePair<string, object> p in Parameters)
                {
                    cacheKeyBuilder.Append(string.Format("[{0}-{1}]", p.Key, p.Value));
                }
                cacheKey = cacheKeyBuilder.ToString();
            }

            return cacheKey;
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

        public event Action OnMappingExecuting;

        protected void OnMappingExecutingHandler()
        {
            Action handler = OnMappingExecuting;
            if (handler != null)
            {
                handler();
            }
        }

        public IResultQuery<TOutput> MappingExecuting(Action args)
        {
            OnMappingExecuting += args;

            return this;
        }

        public event Action OnMappingExecuted;

        protected void OnMappingExecutedHandler()
        {
            Action handler = OnMappingExecuted;
            if (handler != null)
            {
                handler();
            }
        }

        public IResultQuery<TOutput> MappingExecuted(Action args)
        {
            OnMappingExecuted += args;

            return this;
        }

        #endregion
    }
}