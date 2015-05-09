namespace Kassandra.Core.Interfaces
{
    public interface IConnectionProvider
    {
        IConnection BuildConnection(string connectionString);
    }
}