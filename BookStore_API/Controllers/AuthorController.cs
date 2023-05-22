using AutoMapper;
using BookStore_API.Data;
using BookStore_API.Model.Dto;
using BookStore_API.Model;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Microsoft.EntityFrameworkCore;

namespace BookStore_API.Controllers
{
    [Route("api/AuthorController")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public AuthorController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthorDTO>> GetAllAuthors()
        {
            var result = await _db.Authors.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{Id:int}", Name = "GetAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var author = await _db.Authors.SingleOrDefaultAsync(u => u.AuthorID == Id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorDTO>> CreateAuthor([FromBody] AuthorDTO authorCreateDTO)
        {
            // Custom validation for User's Id
            if (await _db.Authors.FirstOrDefaultAsync(u => u.AuthorID == authorCreateDTO.AuthorID) != null)
            {
                ModelState.AddModelError(" ", "Author Name Already Exists");
                return BadRequest(ModelState);
            }
            Author author = _mapper.Map<Author>(authorCreateDTO);

            await _db.Authors.AddAsync(author);
            await _db.SaveChangesAsync();

            return Ok(authorCreateDTO);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDTO authorUpdateDTO)
        {
            if (authorUpdateDTO == null || id != authorUpdateDTO.AuthorID)
            {
                return BadRequest();
            }
            Author model = _mapper.Map<Author>(authorUpdateDTO);

            _db.Authors.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteAuthor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var user = await _db.Authors.FirstOrDefaultAsync(u => u.AuthorID == id);
            if (user == null)
            {
                return NotFound();

            }
            _db.Authors.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
