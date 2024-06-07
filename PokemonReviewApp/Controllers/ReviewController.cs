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
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper,IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews() {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }
        [HttpGet("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200,Type =typeof(Review))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId)) { 
                return NotFound();
            }
            var review= _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult GetReviewOfPokemon(int pokeId) { 
            var pokemon=_mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            return Ok(pokemon);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromQuery] int reviewId,[FromQuery] int pokeId, [FromBody] ReviewDto createReview)
        {
            if (createReview == null)
            {
                return BadRequest(ModelState);
            }
            var reviews = _reviewRepository.GetReviews().Where(c =>
            c.Title.Trim().ToUpper() == createReview.Title.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (reviews != null)
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewMap = _mapper.Map<Review>(createReview);
            reviewMap.Pokemon=_pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer=_reviewerRepository.GetReviewer(reviewId);
            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");

        }
        [HttpPut("{reveiwId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reveiwId, [FromBody] ReviewDto Updatereviw)
        {
            if (Updatereviw == null)
            {
                return BadRequest(ModelState);
            }
            if (reveiwId != Updatereviw.Id)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewMap = _mapper.Map<Review>(Updatereviw);
            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "something went wrong updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
        [HttpDelete("{reveiwId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reveiwId)
        {
            if (!_reviewRepository.ReviewExists(reveiwId)) { 
                return NotFound();
            }
            var review = _reviewRepository.GetReview(reveiwId);
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            if (!_reviewRepository.DeleteReview(review)) {
                ModelState.AddModelError("", "something went wrong deleting review");
                return StatusCode(500, ModelState);
            }
                return NoContent();
        }


    }
}
