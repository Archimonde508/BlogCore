using Blog.DAL.Infrastructure;
using Blog.DAL.Repository;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Blog.DAL.Model;
using TDD.DbTestHelpers.Core;
using TDD.DbTestHelpers;
using TDD.DbTestHelpers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Tests
{

    [TestClass]
    public class RepositoryTests : DbBaseTest<BlogFixtures>
    {
        FileHelper fileHelper = new FileHelper();
        string connectionString;

        public RepositoryTests()
        {
            this.connectionString = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build()
               .GetConnectionString("BloggingDatabase")!;
        }

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
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            fileHelper.FillFixturesFileFiles(context);
            var repository = new BlogRepository(connectionString);

            // act
            var result = repository.GetAllPosts().ToList();

            // assert
            Assert.AreEqual(2, result.Count());

            // to test if pipeline fails
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void AddPost_RequiredFieldIsNull_ThrowsException()
        {
            // arrange
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            fileHelper.FillFixturesFileFiles(context);
            var repository = new BlogRepository(connectionString);
            var newPost = new Post()
            {
            };

            // act
            Assert.ThrowsException<DbUpdateException>(() =>  repository.AddPost(newPost));
        }

        [TestMethod]
        public void AddPost_AllRequiredFieldAreNotNull_AddsPostIntoDb()
        {
            // arrange
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            fileHelper.FillFixturesFileFiles(context);
            var repository = new BlogRepository(connectionString);
            var newPost = new Post()
            {
                Author = "Sean",
                Content = "Blablabla",
            };

            // act
            repository.AddPost(newPost);

            // assert by no exception + verify size of collection
            var length = context.Posts.Count();
            Assert.AreEqual(3, length);
        }

        [TestMethod]
        public void GetAllCommentsForGivenId_WhenPostsHasComments_ReturnsEmptyList()
        {
            // arrange
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            fileHelper.FillFixturesFileFiles(context);
            var repository = new BlogRepository(connectionString);

            // act
            var result = repository.GetAllCommentsForPostId(id: 1).ToList();

            // assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void GetAllCommentsForGivenId_NoCorrespodingComment_ReturnsEmptyList()
        {
            // arrange
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            fileHelper.FillFixturesFileFiles(context);
            var repository = new BlogRepository(connectionString);

            // act
            var result = repository.GetAllCommentsForPostId(id: 2).ToList();

            // assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void AddComment_RequiredFieldIsNull_ThrowsException()
        {
            // arrange
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            fileHelper.FillFixturesFileFiles(context);
            var repository = new BlogRepository(connectionString);
            var newComment = new Comment()
            {
            };

            // act
            Assert.ThrowsException<DbUpdateException>(() => repository.AddComment(newComment));
        }

        [TestMethod]
        public void AddComment_FKDoesNotExist_ThrowsException()
        {
            // arrange
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            fileHelper.FillFixturesFileFiles(context);
            var repository = new BlogRepository(connectionString);
            var newComment = new Comment()
            {
                Id = 6,
                Author = "Obi wan",
                Content = "Bla bla hahahah",
                PostId = 99999
            };

            // act & assert
            Assert.ThrowsException<DbUpdateException>(() => repository.AddComment(newComment));
        }

        [TestMethod]
        public void AddComment_FKKeyExistAndAllRequiredFieldsAreSet_InsertsComment()
        {
            // arrange
            var context = new BlogContext(connectionString);
            context.Database.EnsureCreated();
            fileHelper.FillFixturesFileFiles(context);
            var repository = new BlogRepository(connectionString);
            var newComment = new Comment()
            {
                Id = 9,
                Author = "Obi wan",
                Content = "Bla bla hahahah",
                PostId = 2
            };

            // act 
            repository.AddComment(newComment);

            // Assert
            var comments = context.Comments.Where(x => x.Id == 9).ToList();

            Assert.AreEqual(1, comments.Count());
        }
    }
}
