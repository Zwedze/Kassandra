namespace Kassandra.Users.Core
{
    public interface IRepositoryFactory
    {
        TRepo GetRepository<TRepo>(string connectionString) where TRepo : IRepository;
    }
}