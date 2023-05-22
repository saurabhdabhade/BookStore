using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Model
{
    public class Publisher
    {
        [Key]
        public int PublisherID { get; set; }
        [Required]
        public string PublisherName { get; set;}
        public string PublisherEmail { get; set; }
    }
}
