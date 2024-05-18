using Blog.DAL.Infrastructure;
using Blog.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.DbTestHelpers.Yaml;

namespace TDD.DbTestHelpers
{
    public class BlogFixturesModel
    {
        public FixtureTable<Post> Posts { get; set; }
    }

}
