using System.Collections.Generic;
using System.Configuration;
using Kassandra.Connector.Sql.Factories;
using Kassandra.Connector.Sql.Test.Model;
using Kassandra.Core;
using Kassandra.Core.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kassandra.Connector.Sql.Test
{
    [TestClass]
    public class SampleRequests
    {
        [TestMethod]
        public void GetRequest()
        {
            IContext context = SqlContextFactory.Instance.GetContext(
                ConfigurationManager.ConnectionStrings["Database"].ConnectionString);

            IList<Monster> monsters = context
                .BuildQuery<Monster>("SELECT * FROM Monsters", false)
                .Mapper(new ExpressionMapper<Monster>(
                    new MappingItem<Monster>(x => x.Id, "Id"),
                    new MappingItem<Monster>(x => x.Name, "Name"),
                    new MappingItem<Monster>(x => x.Power, "Power"),
                    new MappingItem<Monster>(x => x.ImageUrl, "Image")
                    ))
                .UseCache()
                .QueryMany();

            foreach (var monster in monsters)
            {
                Assert.AreNotEqual(default(int), monster.Id);
                Assert.AreNotEqual(default(string), monster.ImageUrl);
                Assert.AreNotEqual(default(string), monster.Name);
                Assert.AreNotEqual(default(int), monster.Power);
            }
        }
    }
}