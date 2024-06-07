using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository,IMapper mapper,IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type =typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons() {
            var pokemons= _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            return Ok(pokemons);
        }
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200,Type =typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if(!_pokemonRepository.PokemonExists(pokeId))
                return NotFound(); 
            var pokemon=_mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }
        [HttpGet("{PokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int PokeId) {
            if (!_pokemonRepository.PokemonExists(PokeId)) { 
                return NotFound();

            }
            var rating =_pokemonRepository.GetPokemonRating(PokeId);
            if (!ModelState.IsValid) {
                return BadRequest();

            }
            return Ok(rating);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromQuery] int ownerId,[FromQuery] int categoryId, [FromBody] PokemonDto pokemoncreate)
        {
            if (pokemoncreate == null)
            {
                return BadRequest(ModelState);
            }
            var pokemons = _pokemonRepository.GetPokemons().Where(c =>
            c.Name.Trim().ToUpper() == pokemoncreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (pokemons != null)
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(pokemoncreate);
            
            if (!_pokemonRepository.CreatePokemon(ownerId,categoryId,pokemonMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");

        }
        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId,[FromQuery] int categoryId,[FromQuery]int OwnerId, [FromBody] PokemonDto UpdatePokemon)
        {
            if (UpdatePokemon == null)
            {
                return BadRequest(ModelState);
            }
            if (pokeId != UpdatePokemon.Id)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(UpdatePokemon);
            if (!_pokemonRepository.UpdatePokemon(categoryId,OwnerId,pokemonMap))
            {
                ModelState.AddModelError("", "something went wrong updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) { 
                return NotFound();
            }
            var reivestodelte=_reviewRepository.GetReviewsOfPokemon(pokeId);
            var pokemontodelte= _pokemonRepository.GetPokemon(pokeId);
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            if (!_reviewRepository.DeleteReviews(reivestodelte.ToList())) {
                ModelState.AddModelError("", "SOmething went wrong deleting reviews of a pokemon");
                return StatusCode(500, ModelState);
            }
            if (!_pokemonRepository.DeletePokemon(pokemontodelte)) {
                ModelState.AddModelError("", "something went wrong deleting pokemon");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
