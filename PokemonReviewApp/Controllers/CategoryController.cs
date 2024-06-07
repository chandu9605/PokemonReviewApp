using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository,IMapper mapper) {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories() {
            var Category = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            return Ok(Category);
        }
        [HttpGet("{CategoryId}")]
        [ProducesResponseType(200,Type =typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int CategoryId) {
            if (!_categoryRepository.CategoryExists(CategoryId)) { 
                return NotFound();
            }
            var category=_mapper.Map<CategoryDto>(_categoryRepository.GetCategory(CategoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }
        [HttpGet("pokemon/{CategoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int CategoryId) {
            var pokemon = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategories(CategoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(pokemon);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate) {
            if (categoryCreate == null) { 
                return BadRequest(ModelState);
            }
            var category = _categoryRepository.GetCategories().Where(c=>
            c.Name.Trim().ToUpper()==categoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (category != null) {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var categoryMap = _mapper.Map<Category>(categoryCreate);
            if (!_categoryRepository.CareateCategory(categoryMap)) {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500,ModelState);
            }
            return Ok("Successfully Created");

        }
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto UpdateCategory) {
            if (UpdateCategory == null) {
                return BadRequest(ModelState);
            }
            if (categoryId != UpdateCategory.Id) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            var categoryMap = _mapper.Map<Category>(UpdateCategory);
            if (!_categoryRepository.UpdateCategory(categoryMap)) {
                ModelState.AddModelError("", "something went wrong updating category");
                return StatusCode(500,ModelState);
            }
            return  NoContent();

        }
        [HttpDelete("categoryId")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId)) { 
                return NotFound();
            }
            var categorytodelete = _categoryRepository.GetCategory(categoryId);
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (!_categoryRepository.DeleteCategory(categorytodelete)) {
                ModelState.AddModelError("", "somethinf went wrong deleting category");
                return StatusCode(500,ModelState);
            }
            return NoContent();

        }

    }   
}
