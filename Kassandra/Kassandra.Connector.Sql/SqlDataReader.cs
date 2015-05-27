using System;
using System.Data;
using Kassandra.Core;

namespace Kassandra.Connector.Sql
{
    internal class SqlDataReader : IResultReader
    {
        private readonly IDataReader _dataReader;

        public SqlDataReader(IDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        public T ValueAs<T>(string parameterName)
        {
            if (_dataReader[parameterName] == DBNull.Value)
            {
                return default(T);
            }

            return (T) _dataReader[parameterName];
        }

        public T ValueAs<T>(int columnNumber)
        {
            if (_dataReader[columnNumber] == DBNull.Value)
            {
                return default(T);
            }

            return (T) _dataReader[columnNumber];
        }

        public dynamic ValueAs(Type type, string parameterName)
        {
            if (_dataReader[parameterName] == DBNull.Value)
            {
                return default(dynamic);
            }

            return _dataReader[parameterName];
        }

        public dynamic ValueAs(Type type, int columnNumber)
        {
            if (_dataReader[columnNumber] == DBNull.Value)
            {
                return default(dynamic);
            }

            return _dataReader[columnNumber];
        }

        public bool Read()
        {
            return _dataReader.Read();
        }
    }
}