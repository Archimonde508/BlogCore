using Blog.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Infrastructure
{
    public class BlogContext : DbContext
    {
        private string _connectionString;
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public BlogContext()
        {
            Console.WriteLine("Are we in Blogcontext constructor?");

        }

        public BlogContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)           // Each comment belongs to one post
            .WithMany(p => p.Comments)     // Each post may have many comments
            .HasForeignKey(c => c.PostId) // Define the foreign key property
            .IsRequired();                 // The foreign key is required

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}
