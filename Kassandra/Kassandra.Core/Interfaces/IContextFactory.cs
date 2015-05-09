namespace Kassandra.Core.Interfaces
{
    public interface IContextFactory
    {
        IDataContext GetContext(string connectionString);
    }
}