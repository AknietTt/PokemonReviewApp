using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   [Route("api/CategoryController")]
   [ApiController]
   public class CategoryController:ControllerBase {
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
   }
}
