using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   public class ReviewRepository :IReviewRepository {
      private readonly ApplicationDbContext _context;
      private readonly IMapper _mapper;

      public ReviewRepository(ApplicationDbContext context, IMapper mapper) {
         _context=context;
         _mapper=mapper;
      }

      public bool CreateReview(Review review) {
         _context.Add(review);
         return Save();
      }

      public Review GetReview(int reviewId) {
         return _context.Reviews.FirstOrDefault(e=>e.Id==reviewId);
      }

      public ICollection<Review> GetReviews() {
         return _context.Reviews.ToList();
      }

      public ICollection<Review> GetReviewsOfPokemon(int pokeId) {
         return _context.Reviews.Where(e=>e.Pokemon.Id == pokeId).ToList();   
      }

      public bool ReviewExists(int reviewId) {
         return _context.Reviews.Any(e=>e.Id == reviewId);
      }

      public bool Save() {
         var saved = _context.SaveChanges();
         return saved > 0 ? true : false;
      }

      public bool UpdateReview(Review review) {
         _context.Reviews.Update(review);
         return Save();
      }
   }
}
