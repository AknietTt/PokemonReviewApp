using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository.Interface;

namespace PokemonReviewApp.Repository.Implementation {
   public class CountryRepository :ICountryRepository {
      private readonly IMapper _mapper;
      private readonly ApplicationDbContext _context;

      public CountryRepository(IMapper mapper, ApplicationDbContext context) {
         _mapper=mapper;
         _context=context;
      }

      public bool CountryExists(int id) {
         return _context.Countries.Any(c => c.Id == id);
      }

      public bool CreateCountry(Country country) {
         _context.Add(country);
         return Save(); 
      }

      public ICollection<Country> GetCountries() {
         return _context.Countries.ToList();
      }

      public Country GetCountry(int id) {
         return _context.Countries.FirstOrDefault(c=>c.Id == id); 
      }

      public Country GetCountryByOwner(int ownerId) {
         return _context.Owners.Where(c => c.Id == ownerId).Select(s=>s.Country).FirstOrDefault();
      }

      public ICollection<Owner> GetOwnersFromCounrty(int countryId) {
         return _context.Owners.Where(c=>c.Country.Id == countryId).ToList();
      }

      public bool Save() {
         var saved = _context.SaveChanges();
         return saved > 0?true:false;  
      }

      public bool UpdateCountry(Country country) {
         _context.Update(country);
         return Save();
      }
   }
}
