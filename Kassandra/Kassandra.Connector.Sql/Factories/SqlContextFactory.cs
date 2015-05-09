using Kassandra.Connector.Sql;
using Kassandra.Core;
using Kassandra.Core.Components;

namespace Kassandra.Connector.Sql.Factories
{
    public class SqlContextFactory : IContextFactory
    {
        private static IContextFactory _instance;

        public static IContextFactory Instance
        {
            get { return _instance ?? (_instance = new SqlContextFactory()); }
        }

        public IContext GetContext(string connectionString)
        {
            return new SqlDatabase(connectionString, new CachedRepository());
        }
    }
}