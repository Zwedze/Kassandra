namespace Kassandra.Core
{
    public interface IConnectionProvider
    {
        IConnection BuildConnection(string connectionString);
    }
}