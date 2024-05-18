using Blog.DAL.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TDD.DbTestHelpers.EF;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Blog.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace TDD.DbTestHelpers.Helpers
{
    public class FileHelper
    {
        public void ClearTables<TFixtureType>(DbContext context)
        {
            ClearTables(typeof(TFixtureType), context);
        }

        public void ClearTables(Type fixtureType, DbContext context)
        {
            foreach (var fixtureTable in fixtureType.GetProperties())
            {
                var table = context.GetType().GetProperty(fixtureTable.Name);
                var tableType = table.PropertyType;
                var clearTableMethod = typeof (EfExtensions).GetMethod("ClearTable")
                    .MakeGenericMethod(tableType.GetGenericArguments());
                clearTableMethod.Invoke(null, new[] {table.GetValue(context, null)});
            }
            context.SaveChanges();
        }
        
        public void FillFixturesFileFiles<TFixtureType>(BlogContext context, string yamlFolderName, IEnumerable<string> yamlFilesNames)
        {
             
        }

        public void FillFixturesFileFiles(BlogContext context)
        {
            string jsonFilePath = "Fixtures\\posts.json";
            string jsonContent = File.ReadAllText(jsonFilePath);
            Post[] posts = JsonConvert.DeserializeObject<Post[]>(jsonContent);

            var allComments = context.Comments.ToList();
            context.Comments.RemoveRange(allComments);

            var allPosts = context.Posts.ToList();
            context.Posts.RemoveRange(allPosts);

            foreach (var post in posts)
            {
                context.Posts.Add(post);
            }

            string commentsJsonFilePath = "Fixtures\\comments.json";
            string commentsJsonContent = File.ReadAllText(commentsJsonFilePath);
            Comment[] comments = JsonConvert.DeserializeObject<Comment[]>(commentsJsonContent);


            // We cannot insert PK into database.
            foreach (var comment in comments)
            {
                context.Comments.Add(comment);
            }

            context.SaveChanges();


        }

    }
}
