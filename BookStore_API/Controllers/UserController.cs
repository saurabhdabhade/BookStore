using AutoMapper;
using BookStore_API.Model;
using BookStore_API.Model.Dto;
using BookStore_API.Model.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookStore_API.Controllers
{
    [Route("api/UserController")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public UserController(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllUsers()
        {
            try
            {
                IEnumerable<User> userList = await _repository.GetAllAsync();
                _response.Result = _mapper.Map<List<UserDTO>>(userList);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUser(int UserId)
        {
            try
            {
                if (UserId == 0)
                {
                    return BadRequest();
                }
                var user = await _repository.GetAsync(u => u.UserId == UserId);

                if (user == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                 = new List<string>() { ex.Message };
            }
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO createDTO)
        {
            try
            {
                if (await _repository.GetAsync(u => u.UserId == createDTO.UserId) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "User Already Exist!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                User user = _mapper.Map<User>(createDTO);

                await _repository.CreateAsync(user);
                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetUser", new { id = user.UserId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                 = new List<string>() { ex.ToString()};
            }
            return Ok(_response);
        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteUser(int UserId)
        {
            try
            {
                if (UserId == 0)
                {
                    return BadRequest();
                }

                var user = await _repository.GetAsync(u => u.UserId == UserId);

                if (user == null)
                {
                    return NotFound();
                }
                await _repository.RemoveAsync(user);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.Message };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateUser(int id, [FromBody] UserDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.UserId)
                {
                    return BadRequest();
                }

                User model = _mapper.Map<User>(updateDTO);

                var user = await _repository.GetAsync(u => u.UserId == id);    //Update Is Not Working

                _repository.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.Message };
            }
            return _response;
        }
    }
}
