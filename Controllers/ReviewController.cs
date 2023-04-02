﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Controllers {
   [Route("api/ReviewController")]
   [ApiController]
   public class ReviewController:ControllerBase {
      private readonly IReviewRepository _reviewRepository;
      private readonly IMapper _mapper;

      public ReviewController(IReviewRepository reviewRepository, IMapper mapper) {
         _reviewRepository=reviewRepository;
         _mapper=mapper;
      }

      [HttpGet]
      [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
      public IActionResult GetReviews() {
         var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         return Ok(reviews);
      }

      [HttpGet("{reviewId}")]
      [ProducesResponseType(200, Type = typeof(Review))]
      [ProducesResponseType(400)]
      public IActionResult GetPokemon(int reviewId) {
         if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();

         var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

         if (!ModelState.IsValid)
            return BadRequest(ModelState);

         return Ok(review);
      }

      [HttpGet("pokemon/{pokeId}")]
      [ProducesResponseType(200, Type = typeof(Review))]
      [ProducesResponseType(400)]
      public IActionResult GetReviewsForAPokemon(int pokeId) {
         var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfPokemon(pokeId));

         if (!ModelState.IsValid)
            return BadRequest();

         return Ok(reviews);
      }
   }
}