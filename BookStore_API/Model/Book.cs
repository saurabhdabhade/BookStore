using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Model
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set;}
        [Required]
        [ForeignKey("Author")]
        public int AuthorID { get; set; }
        public Author? Author { get; set; }

        [ForeignKey("Publisher")]
        public int PublisherID { get; set; }
        public Publisher Publisher { get; set; }

        [Required]
        public string PublicationYear { get; set; }
    }
}
