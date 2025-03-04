﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context) { 
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
           var pokemonOwnerEntity= _context.Owners.Where(a=>a.Id==ownerId).FirstOrDefault();
            var category=_context.Categories.Where(c=>c.Id==categoryId).FirstOrDefault();
            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,

            };
            _context.Add(pokemonOwner);
            var pokemoncategory = new PokemonCategory() {
                Category=category,
                Pokemon=pokemon,
            };
            _context.Add(pokemoncategory);
            _context.Add(pokemon);
            return Save();



        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int Id)
        {
            return _context.Pokemon.Where(p => p.Id == Id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string Name)

        {
            return _context.Pokemon.Where(p => p.Name == Name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int PokeId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == PokeId);
            if (review.Count() <= 0) 
                return 0;
            
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p=>p.Id).ToList();
        }

        public bool PokemonExists(int Pokeid)
        {
            return _context.Pokemon.Any(p => p.Id == Pokeid);
        }

        public bool Save()
        {
            var save=_context.SaveChanges();
            return save>0?true:false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
