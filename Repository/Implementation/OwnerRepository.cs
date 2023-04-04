using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   public class OwnerRepository :IOwnerRepository {
      private readonly IMapper _mapper;
      private readonly ApplicationDbContext _context;

      public OwnerRepository(IMapper mapper, ApplicationDbContext context) {
         _mapper=mapper;
         _context=context;
      }

      public bool CreateOwner(Owner owner) {
         _context.Add(owner);
         return Save();
      }

      public ICollection<Owner> GetAllOwnerOfPokemon(int pokeId) {
         return _context.PokemonOwners.Where(o => o.Pokemon.Id == pokeId).Select(o => o.Owner).ToList();
      }

      public Owner GetOwner(int ownerId) {
        return _context.Owners.FirstOrDefault(p => p.Id == ownerId); 
      }

      public ICollection<Owner> GetOwners() {
         return _context.Owners.ToList();
      }

      public ICollection<Pokemon> GetPokemonByOwner(int ownerId) {
         return _context.PokemonOwners.Where(p => p.Owner.Id == ownerId).Select(s => s.Pokemon).ToList();
      }

      public bool OwnerExists(int ownerId) {
         return _context.Owners.Any(o=>o.Id == ownerId);
      }

      public bool Save() {
         var saved = _context.SaveChanges();
         return saved > 0 ? true : false;
      }

      public bool UpdateOwner(Owner owner) {
         _context.Owners.Update(owner);
         return Save();
      }
   }
}
