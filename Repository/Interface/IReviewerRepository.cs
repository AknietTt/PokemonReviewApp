﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository.Interface {
   public interface IReviewerRepository {
      ICollection<Reviewer> GetReviewers();
      Reviewer GetReviewer(int reviewerId);
      ICollection<Review> GetReviewsByReviewer(int reviewerId);
      bool ReviewerExists(int reviewerId);
   }
}
