using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository.Interface {
   public interface ICountryRepository {
      ICollection<Country> GetCountries();
      Country GetCountry(int id);
      Country GetCountryByOwner(int ownerId);
      ICollection<Owner> GetOwnersFromCounrty(int countryId);
      bool CountryExists(int id);
   }
}
