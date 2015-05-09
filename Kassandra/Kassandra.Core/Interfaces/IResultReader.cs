namespace Kassandra.Core.Interfaces
{
    public interface IResultReader
    {
        T ValueAs<T>(string parameterName);
        T ValueAs<T>(int columnNumber);
        bool Read();
    }
}