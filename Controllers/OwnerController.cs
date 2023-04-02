﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Controllers {
   [Route("api/OwnerController")]
   [ApiController]
   public class OwnerController:ControllerBase {
      private readonly IOwnerRepository _ownerRepository;
      private readonly ICountryRepository _countryRepository;
      private readonly IMapper _mapper;

      public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository) {
         _ownerRepository=ownerRepository;
         _mapper=mapper;
         _countryRepository=countryRepository;
      }

      [HttpGet]
      [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
      public IActionResult GetOwners() {
         var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(owners);
      }

      [HttpGet("{ownerId}")]
      [ProducesResponseType(200, Type = typeof(Owner))]
      [ProducesResponseType(400)]
      public IActionResult GetOwner(int ownerId) {
         if (!_ownerRepository.OwnerExists(ownerId)) {
            return NotFound();
         }
         var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

         if (!ModelState.IsValid) {
            return BadRequest(ModelState);
         }
         return Ok(owner);
      }

      [HttpGet("{ownerId}/pokemon")]
      [ProducesResponseType(200, Type = typeof(Owner))]
      [ProducesResponseType(400)]
      public IActionResult GetPokemonByOwner(int ownerId) {
         if (!_ownerRepository.OwnerExists(ownerId)) {
            return NotFound();
         }

         var owner = _mapper.Map<List<PokemonDto>>(
             _ownerRepository.GetPokemonByOwner(ownerId));

         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         return Ok(owner);
      }

      [HttpPost]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      public IActionResult CreateOwner([FromQuery] int countryID, [FromBody] OwnerDto ownerCreate) {
         if (ownerCreate == null) {
            return BadRequest(ModelState);
         }
         var owner = _ownerRepository.GetOwners().
            Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
            .FirstOrDefault();

         if (owner != null) {
            ModelState.AddModelError("", "owner already exists");
            return StatusCode(422, ModelState);
         }

         if (!ModelState.IsValid) {
            return BadRequest();
         }

         var ownerMap = _mapper.Map<Owner>(ownerCreate);
         ownerMap.Country = _countryRepository.GetCountry(countryID); 

         if (!_ownerRepository.CreateOwner(ownerMap)) {
            ModelState.AddModelError("", "Somthing went wrong while savin");
            return StatusCode(500, ModelState);
         }

         return Ok("Successfully created");
      }
   }
}
