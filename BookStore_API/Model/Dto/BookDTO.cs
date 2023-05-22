using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Model.Dto
{
    public class BookDTO
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set;}
        public int AuthorID { get; set; }
        public int PublisherID { get; set; }
        public string PublicationYear { get; set; }
    }
}
