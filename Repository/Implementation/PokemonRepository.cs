using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   public class PokemonRepository :IPokemonRepository {
      private readonly ApplicationDbContext _context;

      public PokemonRepository(ApplicationDbContext context) {
         _context=context;
      }

      public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon) {
         var pokemonOwnerEntity = _context.Owners.Where(x=>x.Id == ownerId).FirstOrDefault();
         var category = _context.Categories.Where(x=>x.Id == categoryId).FirstOrDefault();

         var pokemonOwner = new PokemonOwner() {
            Owner = pokemonOwnerEntity,
            Pokemon = pokemon
         };

         _context.Add(pokemonOwner);

         var pokemonCategory = new PokemonCategory() {
            Category = category,
            Pokemon = pokemon,
         };

         _context.Add(pokemonCategory);
         _context.Add(pokemon);
         return Save();
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

      public bool Save() {
         var saved = _context.SaveChanges();
         return saved > 0 ? true : false;
      }

      public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon) {
         _context.Update(pokemon);
         return Save();
      }
   }
}
