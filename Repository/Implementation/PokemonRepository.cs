using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   public class PokemonRepository :IPokemonRepository {
      private readonly ApplicationDbContext _context;

      public PokemonRepository(ApplicationDbContext context) {
         _context=context;
      }

      public Pokemon GetPokemon(int id) {
         return _context.Pokemons.FirstOrDefault(p => p.Id == id);   
      }

      public Pokemon GetPokemon(string name) {
         return _context.Pokemons.FirstOrDefault(p=>p.Name == name);
      }

      public decimal GetPokemonRating(int id) {
         var review = _context.Reviews.Where(review => review.Pokemon.Id == id);
         if(review.Count() <= 0) {
            return 0;
         }
         return (decimal)(review.Sum(r => r.Rating) / review.Count());
      }

      public ICollection<Pokemon> GetPokemons() {
         return _context.Pokemons.ToList();
      }

      public bool PokemonExists(int id) {
         return _context.Pokemons.Any(p => p.Id == id);
      }
   }
}
