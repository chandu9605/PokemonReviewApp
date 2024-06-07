using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers() {
            var reviewer = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviewer);
        }
        [HttpGet("{ReviewersId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int ReviewersId) {
            if (!_reviewerRepository.ReviwerExists(ReviewersId)) {
                return NotFound();
            }
            var reveiwer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(ReviewersId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reveiwer);
        }
        [HttpGet("Review/{ReviewersId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewsByReviewer(int ReviewersId)
        {
            var reveiwer = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(ReviewersId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(reveiwer);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] ReviewerDto createReviewer)
        {
            if (createReviewer == null)
            {
                return BadRequest(ModelState);
            }
            var reviewer = _reviewerRepository.GetReviewers().Where(c =>
            c.LastName.Trim().ToUpper() == createReviewer.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (reviewer != null)
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewerMap = _mapper.Map<Reviewer>(createReviewer);
          
            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");

        }
        [HttpPut("{reveiwerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reveiwerId, [FromBody] ReviewerDto Updatereviwer)
        {
            if (UpdateReviewer == null)
            {
                return BadRequest(ModelState);
            }
            if (reveiwerId != Updatereviwer.Id)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewerMap = _mapper.Map<Reviewer>(Updatereviwer);
            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "something went wrong updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReveiwer( int reviewerId) {
            if (!_reviewerRepository.ReviwerExists(reviewerId)) { 
                return NotFound ();

            }
            var reviewer= _reviewerRepository.GetReviewer(reviewerId);
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            if (!_reviewerRepository.DeleteReviewer(reviewer)) {
                ModelState.AddModelError("", "something went wrong deleting reviewer");
                return StatusCode(500, ModelState);
            }
            return NoContent() ;
        }

    }
}
