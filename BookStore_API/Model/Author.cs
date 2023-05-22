using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Model
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        [Required]
        public string? AuthorName { get; set; }
        public string AuthorEmail { get; set; }
    }
}
