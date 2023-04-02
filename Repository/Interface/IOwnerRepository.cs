﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository.Interface {
   public interface IOwnerRepository {
      ICollection<Owner> GetOwners();
      Owner GetOwner(int ownerId);
      ICollection<Owner> GetAllOwnerOfPokemon(int pokeId);
      ICollection<Pokemon> GetPokemonByOwner(int ownerId);
      bool OwnerExists(int ownerId);
   }
}
