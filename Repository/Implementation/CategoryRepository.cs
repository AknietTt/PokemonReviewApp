using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   public class CategoryRepository :ICategoryRepository {
      private readonly ApplicationDbContext _context;

      public CategoryRepository(ApplicationDbContext context) {
         _context = context;
      }


      public bool CategoryExists(int id) {
         return  _context.Categories.Any(c => c.Id == id);
      }

      public ICollection<Category> GetCategories() {
         return _context.Categories.ToList();
      }

      public Category GetCategory(int id) {
         return _context.Categories.FirstOrDefault(c=> c.Id == id);
      }

      public ICollection<Pokemon> GetPokemonByCategories(int categoryId) {
         return _context.PokemonCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
      }
   }
}
