﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository.Interface {
   public interface ICategoryRepository {
      ICollection<Category> GetCategories();  
      Category GetCategory(int id);
      ICollection<Pokemon> GetPokemonByCategories(int categoryId);
      bool CategoryExists(int id);
      bool CreateCategory(Category category);
      bool UpdateCategory(Category category);
      bool DeleteCategory(Category category);
      bool Save();
   }
}
