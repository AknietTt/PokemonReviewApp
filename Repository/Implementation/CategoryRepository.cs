using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   public class CategoryRepository :ICategoryRepository {
      private readonly ApplicationDbContext _context;
      private readonly IMapper _mapper;


      public CategoryRepository(ApplicationDbContext context, IMapper mapper) {
         _context = context;
         _mapper=mapper;
      }


      public bool CategoryExists(int id) {
         return  _context.Categories.Any(c => c.Id == id);
      }

      public bool CreateCategory(Category category) {
         _context.Add(category);
         return Save();
      }

      public bool DeleteCategory(Category category) {
         _context.Remove(category);
         return Save();
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

      public bool Save() {
         var saved = _context.SaveChanges();
         return saved > 0 ? true:false ;
      }

      public bool UpdateCategory(Category category) {
         _context.Update(category); 
         return Save();
      }
   }
}
