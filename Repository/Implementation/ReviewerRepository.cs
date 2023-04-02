using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   public class ReviewerRepository :IReviewerRepository {
      private readonly ApplicationDbContext _context;
      private readonly IMapper _mapper;

      public ReviewerRepository(ApplicationDbContext context, IMapper mapper) {
         _context = context;
         _mapper = mapper;
      }

      public bool CreateReviewer(Reviewer reviewer) {
         _context.Reviwers.Add(reviewer);
         return Save();
      }

      public Reviewer GetReviewer(int reviewerId) {
         return _context.Reviwers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
      }

      public ICollection<Reviewer> GetReviewers() {
         return _context.Reviwers.ToList();
      }

      public ICollection<Review> GetReviewsByReviewer(int reviewerId) {
         return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
      }

      public bool ReviewerExists(int reviewerId) {
         return _context.Reviwers.Any(r => r.Id == reviewerId);
      }

      public bool Save() {
         var saved = _context.SaveChanges();
         return saved > 0 ? true : false;
      }
   }
}
