using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Controllers {
   [Route("api/CountryController")]
   [ApiController]
   public class CountryController:ControllerBase {
      private readonly ICountryRepository _countryRepository;
      private readonly IMapper _mapper;

      public CountryController(ICountryRepository categoryRepository, IMapper mapper) {
         _countryRepository=categoryRepository;
         _mapper=mapper;
      }

     
      [HttpGet]
      [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
      public IActionResult GetCountries() {
         var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(countries);
      }


      [HttpGet("{countryId}")]
      [ProducesResponseType(200, Type = typeof(Country))]
      [ProducesResponseType(400)]
      public IActionResult GetCountry(int countryId) {
         if (!_countryRepository.CountryExists(countryId)) {
            return NotFound();
         }
         var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(country);

      }

      [HttpGet("owners/{ownerId}")]
      [ProducesResponseType(400)]
      [ProducesResponseType(200, Type = typeof(Country))]
      public IActionResult GetCountryOfAnOwner(int ownerId) {
         var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(country);
      }

      [HttpPost]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      public IActionResult CreateCountry([FromBody] CountryDto countryCraete) {
         if (countryCraete == null) {
            return BadRequest(ModelState);
         }
         var country = _countryRepository.GetCountries().
            Where(c => c.Name.Trim().ToUpper() == countryCraete.Name.TrimEnd().ToUpper())
            .FirstOrDefault();
         if (country != null) {
            ModelState.AddModelError("", "country already exists");
            return StatusCode(422, ModelState);
         }

         if (!ModelState.IsValid) {
            return BadRequest();
         }

         var countryMap = _mapper.Map<Country>(countryCraete);
         if (!_countryRepository.CreateCountry(countryMap)) {
            ModelState.AddModelError("", "Somthing went wrong while savin");
            return StatusCode(500, ModelState);
         }

         return Ok("Successfully created");
      }

      [HttpPut("{countryId}")]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      [ProducesResponseType(404)]
      public IActionResult UpdateCategory(int countryId, [FromBody] CountryDto countryUpdate) {
         if (countryUpdate == null) {
            return BadRequest(ModelState);
         }
         if (countryId != countryUpdate.Id) {
            return BadRequest(ModelState);
         }

         if (!_countryRepository.CountryExists(countryId)) {
            return NotFound();
         }

         if (!ModelState.IsValid) {
            return BadRequest();
         }

         var categoryMap = _mapper.Map<Country>(countryUpdate);

         if (!_countryRepository.UpdateCountry(categoryMap)) {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
         }

         return NoContent();
      }

      [HttpDelete("{countryId}")]
      [ProducesResponseType(400)]
      [ProducesResponseType(204)]
      [ProducesResponseType(404)]
      public IActionResult DeleteCountry(int countryId) {
         if (!_countryRepository.CountryExists(countryId)) {
            return NotFound();
         }

         var countryToDelete = _countryRepository.GetCountry(countryId);

         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         if (!_countryRepository.DeleteCountry(countryToDelete)) {
            ModelState.AddModelError("", "Something went wrong deleting category");
         }

         return NoContent();
      }
   }
}
