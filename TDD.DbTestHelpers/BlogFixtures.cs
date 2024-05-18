using Blog.DAL.Infrastructure;
using TDD.DbTestHelpers.Yaml;

namespace TDD.DbTestHelpers
{
    public class BlogFixtures
        : YamlDbFixture<BlogContext, BlogFixturesModel>
    { 

        public BlogFixtures()
        {
            SetYamlFiles("posts.yaml");
        }
    }

}
