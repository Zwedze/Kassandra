using System;

namespace Kassandra.Core
{
    public interface IResultReader
    {
        T ValueAs<T>(string parameterName);
        T ValueAs<T>(int columnNumber);
        dynamic ValueAs(Type type, string parameterName);
        dynamic ValueAs(Type type, int columnNumber);
        bool Read();
    }
}