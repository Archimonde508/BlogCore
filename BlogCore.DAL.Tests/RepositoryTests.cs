using Blog.DAL.Infrastructure;
using Blog.DAL.Repository;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Blog.DAL.Tests
{

    [TestClass]
    public class RepositoryTests
    {

        public static string GetConnectionString(string name)
        {
            Configuration config =
            ConfigurationManager.OpenExeConfiguration(
            ConfigurationUserLevel.None);
            ConnectionStringsSection csSection =
            config.ConnectionStrings;
            for (int i = 0; i <
            ConfigurationManager.ConnectionStrings.Count; i++)
            {
                ConnectionStringSettings cs =
                csSection.ConnectionStrings[i];
                if (cs.Name == name)
                {
                    return cs.ConnectionString;
                }
            }
            return "";
        }

        [TestMethod]
        public void GetAllPost_OnePostInDb_ReturnOnePost()
        {
            // arrange
            String connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("BloggingDatabase");
            //String connectionString = GetConnectionString("BloggingDatabase");
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            var repository = new BlogRepository(connectionString);
            // act
            var result = repository.GetAllPosts();
            // assert
            Assert.AreEqual(1, result.Count());
        }
    }
}
