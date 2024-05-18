using System.Collections.Generic;
using Blog.DAL.Infrastructure;
using Blog.DAL.Model;
using System;

namespace Blog.DAL.Repository
{
    public class BlogRepository
    {
        private readonly BlogContext _context;

        public BlogRepository(string connectionString)
        {
            _context = new BlogContext(connectionString);
        }

        public IEnumerable<Comment> GetAllCommentsForPostId(int id)
        {
            var comments = _context.Comments
                .Where(c => c.PostId == id);

            return comments;
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _context.Posts;
        }

        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
    }
}
