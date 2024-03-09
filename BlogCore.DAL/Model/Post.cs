using System.ComponentModel.DataAnnotations;

namespace Blog.DAL.Model
{
    public class Post
    {
        [Key]
        public long Id { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }
    }
}
