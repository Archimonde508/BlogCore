using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Model
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Author { get; set; }

        // Foreign key to establish the relationship with the corresponding post
        public long PostId { get; set; }
        public virtual Post Post { get; set; }

    }
}
