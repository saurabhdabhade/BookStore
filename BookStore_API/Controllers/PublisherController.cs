using AutoMapper;
using BookStore_API.Model.Dto;
using BookStore_API.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BookStore_API.Model.Repository;

namespace BookStore_API.Controllers
{
    [Route("api/PublisherController")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IRepository<Publisher> _publisherRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public PublisherController(IRepository<Publisher> publisherrepo, IMapper mapper)
        {
            _publisherRepository = publisherrepo;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllPublishers()
        {
            try
            {
                IEnumerable<Publisher> publisherList = await _publisherRepository.GetAllAsync();
                _response.Result = _mapper.Map<List<PublisherDTO>>(publisherList);
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

        [HttpGet("{id:int}", Name = "GetPublisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublisherDTO>> GetPublisher(int PublisherId)
        {
            try
            {
                if (PublisherId == 0)
                {
                    return BadRequest();
                }
                var publisher = await _publisherRepository.GetAsync(u => u.PublisherID == PublisherId);

                if (publisher == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<PublisherDTO>(publisher);
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
        public async Task<ActionResult<PublisherDTO>> CreatePublisher([FromBody] PublisherDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Publisher publisher = _mapper.Map<Publisher>(createDTO);

                await _publisherRepository.CreateAsync(publisher);
                _response.Result = _mapper.Map<PublisherDTO>(publisher);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetUser", new { id = publisher.PublisherID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                 = new List<string>() { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpDelete("{id:int}", Name = "DeletePublisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePublisher(int PublisherId)
        {
            try
            {
                if (PublisherId == 0)
                {
                    return BadRequest();
                }

                var publisher = await _publisherRepository.GetAsync(u => u.PublisherID == PublisherId);

                if (publisher == null)
                {
                    return NotFound();
                }
                await _publisherRepository.RemoveAsync(publisher);
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

        [HttpPut("{id:int}", Name = "UpdatePublisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePublisher(int PublisherId, [FromBody] PublisherDTO updateDTO)
        {
            try
            {
                if (updateDTO == null) 
                {
                    return BadRequest();
                }

                Publisher model = _mapper.Map<Publisher>(updateDTO);

                var publisher = await _publisherRepository.GetAsync(u => u.PublisherID == PublisherId);

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
