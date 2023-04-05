using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   [Route("api/CategoryController")]
   [ApiController]
   public class CategoryController :ControllerBase {
      private readonly ICategoryRepository _categoryRepository;
      private readonly IMapper _mapper;

      public CategoryController(ICategoryRepository pokemonRepository, IMapper mapper) {
         _categoryRepository=pokemonRepository;
         _mapper=mapper;
      }

      [HttpGet]
      [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
      public IActionResult GetCategories() {
         var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(categories);
      }

      [HttpGet("{cateId}")]
      [ProducesResponseType(200, Type = typeof(Category))]
      [ProducesResponseType(400)]
      public IActionResult GetCategory(int cateId) {
         if (!_categoryRepository.CategoryExists(cateId)) {
            return NotFound();
         }
         var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(cateId));

         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(category);

      }

      [HttpGet("pokemon/{cateId}")]
      [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
      [ProducesResponseType(400)]
      public IActionResult GetPokemonByCategory(int cateId) {
         var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategories(cateId));
         if (!ModelState.IsValid) {
            return BadRequest();
         }
         return Ok(pokemons);
      }

      [HttpPost]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      public IActionResult CreateCategory([FromBody] CategoryDto categoryCraete) {
         if (categoryCraete == null) {
            return BadRequest(ModelState);
         }
         var category = _categoryRepository.GetCategories().
            Where(c => c.Name.Trim().ToUpper() == categoryCraete.Name.TrimEnd().ToUpper())
            .FirstOrDefault();
         if (category != null) {
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(422, ModelState);
         }

         if (!ModelState.IsValid) {
            return BadRequest();
         }

         var categoryMap = _mapper.Map<Category>(categoryCraete);
         if (!_categoryRepository.CreateCategory(categoryMap)) {
            ModelState.AddModelError("", "Somthing went wrong while savin");
            return StatusCode(500, ModelState);
         }

         return Ok("Successfully created");
      }

      [HttpPut("{categoryId}")]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      [ProducesResponseType(404)]
      public IActionResult UpdateCategory(int categoryId,[FromBody] CategoryDto categoryUpdate) {
         if (categoryUpdate == null) {
            return BadRequest(ModelState);
         }
         if (categoryId != categoryUpdate.Id) {
            return BadRequest(ModelState);
         }

         if (!_categoryRepository.CategoryExists(categoryId)) { 
            return NotFound();   
         }

         if (!ModelState.IsValid) {
            return BadRequest();
         }

         var categoryMap = _mapper.Map<Category>(categoryUpdate);

         if (!_categoryRepository.UpdateCategory(categoryMap)) {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
         }

         return NoContent();
      }

      [HttpDelete("{categoryId}")]
      [ProducesResponseType(400)]
      [ProducesResponseType(204)]
      [ProducesResponseType(404)]
      public IActionResult DeleteCategory(int categoryId) {
         if (!_categoryRepository.CategoryExists(categoryId)) {
            return NotFound();
         }

         var categoryToDelete = _categoryRepository.GetCategory(categoryId);

         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         if (!_categoryRepository.DeleteCategory(categoryToDelete)) {
            ModelState.AddModelError("", "Something went wrong deleting category");
         }

         return NoContent();
      }
   }
}
