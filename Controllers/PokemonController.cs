
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Controllers {

   [Route("api/PokemonController")]
   [ApiController]
   public class PokemonController:ControllerBase {
      private readonly IPokemonRepository _pokemonRepository;
      private readonly IMapper _mapper;
      private readonly IOwnerRepository _ownerRepository;
      private readonly IReviewRepository _reviewRepository;

      public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, IOwnerRepository ownerRepository, IReviewRepository reviewRepository) {
         this._pokemonRepository = pokemonRepository;
         _mapper = mapper;
         _ownerRepository=ownerRepository;
         _reviewRepository=reviewRepository;
      }

      [HttpGet]
      [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
      public IActionResult GetPokemons() {
         var pokemons = _mapper.Map<List<PokemonDto>> (_pokemonRepository.GetPokemons());
         if (!ModelState.IsValid) {
            return BadRequest(ModelState); 
         }
         return Ok(pokemons);
      }

      [HttpGet("{pokeId}")]
      [ProducesResponseType(200, Type = typeof(Pokemon))]
      [ProducesResponseType(400)]
      public IActionResult GetPokemon(int pokeId) {
         if (!_pokemonRepository.PokemonExists(pokeId)) {
            return NotFound();
         }
         var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));

         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(pokemon);
         
      }

      [HttpGet("{pokeId}/rating")]
      [ProducesResponseType(200, Type = typeof(Pokemon))]
      [ProducesResponseType(400)]
      public IActionResult GetPokemonRating(int pokeId) {
         if (!_pokemonRepository.PokemonExists(pokeId)) {
            return NotFound();
         }
         var rating = _pokemonRepository.GetPokemonRating(pokeId);

         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(rating);
      }


      [HttpPost]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto pokemonCreate) {
         if (pokemonCreate == null) {
            return BadRequest(ModelState);
         }

         var pokemons = _pokemonRepository.GetPokemons().
            Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper())
            .FirstOrDefault();

         if (pokemons != null) {
            ModelState.AddModelError("", "pokemon already exists");
            return StatusCode(422, ModelState);
         }

         if (!ModelState.IsValid) {
            return BadRequest();
         }

         var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

         if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap)) {
            ModelState.AddModelError("", "Somthing went wrong while savin");
            return StatusCode(500, ModelState);
         }

         return Ok("Successfully created");
      }

      [HttpPut("{pokeId}")]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      [ProducesResponseType(404)]
      public IActionResult UpdatePokemon(int pokeId, [FromQuery] int ownerId,[FromQuery] int catId, [FromBody] PokemonDto pokemonUpdate) {
         if (pokemonUpdate == null) {
            return BadRequest(ModelState);
         }
         if (pokeId != pokemonUpdate.Id) {
            return BadRequest(ModelState);
         }

         if (!_pokemonRepository.PokemonExists(pokeId)) {
            return NotFound();
         }

         if (!ModelState.IsValid) {
            return BadRequest();
         }

         var pokemonMap = _mapper.Map<Pokemon>(pokemonUpdate);

         if (!_pokemonRepository.UpdatePokemon(ownerId,catId, pokemonMap)) {
            ModelState.AddModelError("", "Something went wrong updating Pokemon");
            return StatusCode(500, ModelState);
         }

         return NoContent();
      }

      [HttpDelete("{pokeId}")]
      [ProducesResponseType(400)]
      [ProducesResponseType(204)]
      [ProducesResponseType(404)]
      public IActionResult DeletePokemon(int pokeId) {
         if (!_pokemonRepository.PokemonExists(pokeId)) {
            return NotFound();
         }

         var reviewsToDelete = _reviewRepository.GetReviewsOfPokemon(pokeId);
         var pokemonToDelete = _pokemonRepository.GetPokemon(pokeId);

         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList())) {
            ModelState.AddModelError("", "Something went wrong when deleting reviews");
         }

         if (!_pokemonRepository.DeletePokemon(pokemonToDelete)) {
            ModelState.AddModelError("", "Something went wrong deleting owner");
         }

         return NoContent();
      }
   }
}
