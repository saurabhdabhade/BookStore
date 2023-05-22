using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Model.Dto
{
    public class AuthorDTO
    {
        public int AuthorID { get; set; }
        [Required]
        public string? AuthorName { get; set; }
        public string AuthorEmail { get; set; }
    }
}
