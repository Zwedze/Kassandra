using System;
using Kassandra.Users.Core;

namespace Kassandra.Users.Sql.Factories
{
    public class SqlRepositoryFactory : IRepositoryFactory
    {
        private static IRepositoryFactory _instance;

        public static IRepositoryFactory Instance
        {
            get { return _instance ?? (_instance = new SqlRepositoryFactory()); }
        }

        public TRepo GetRepository<TRepo>(string connectionString) where TRepo : IRepository
        {
            Type repositoryType = typeof (TRepo);

            if (!repositoryType.IsInterface)
            {
                throw new ArgumentException("Repository type is not an interface");
            }

            switch (repositoryType.FullName)
            {
                case "Kassandra.Users.Core.IUserRepository":
                    return (TRepo)(new UserRepository(connectionString) as IUserRepository);
                case "Kassandra.Users.Core.IRoleRepository":
                    return (TRepo)(new RoleRepository(connectionString) as IRoleRepository);
                default:
                    throw new ArgumentException(
                        string.Format(
                            "Repository type is not valid. Type provided is : {0}. Namespace should be 'Kassandra.Users.Core'",
                            repositoryType.FullName));
            }
        }
    }
}