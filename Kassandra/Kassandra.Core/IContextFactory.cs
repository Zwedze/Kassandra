namespace Kassandra.Core
{
    public interface IContextFactory
    {
        IContext GetContext(string connectionString);
    }
}