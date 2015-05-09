using System;
using System.Data;
using Kassandra.Core.Interfaces;

namespace Kassandra.Connector.Sql.Implementation
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

        public bool Read()
        {
            return _dataReader.Read();
        }
    }
}