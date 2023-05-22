using AutoMapper;
using BookStore_API.Data;
using BookStore_API.Model;
using BookStore_API.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore_API.Controllers
{
    [Route("api/BookController")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        //private readonly Author _author;
        //private readonly Publisher _publisher;
        public BookController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            //_author = author;
            //_publisher = publisher;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllBooks()
        {
            var result = _db.Books.ToList();
            return Ok(result);
        }

        [HttpGet("{Id:int}", Name = "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBook(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var book = await _db.Books.SingleOrDefaultAsync(u => u.Id == Id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDTO>> CreateBook([FromBody] BookDTO bookCreateDTO)
        {
            if (await _db.Books.SingleOrDefaultAsync(u => u.Id == bookCreateDTO.Id) != null)
            {
                ModelState.AddModelError(" ", "Book ID Already Exists!");
                return BadRequest(ModelState);
            }
           
            Book model = _mapper.Map<Book>(bookCreateDTO);
            _db.Books.AddAsync(model);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDTO bookUpdateDTO)
        {
            if (bookUpdateDTO == null || id != bookUpdateDTO.Id)
            {
                return BadRequest();
            }
            Book model = _mapper.Map<Book>(bookUpdateDTO);

            _db.Books.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var user = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();

            }
            _db.Books.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
