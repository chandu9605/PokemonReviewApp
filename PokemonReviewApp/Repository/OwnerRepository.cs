using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault(); 
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int PokeID)
        {
            return _context.PokemonOwners.Where(p =>p.Pokemon.Id==PokeID).Select(o=>o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonBYOwner(int OwnerID)
        {
            return _context.PokemonOwners.Where(p=>p.Owner.Id==OwnerID).Select(p=>p.Pokemon).ToList();
        }

        public bool OwnerExists(int OwnerID)
        {
            return _context.Owners.Any(o => o.Id == OwnerID);
        }

        public bool Save()
        {
            var save=_context.SaveChanges();
            return save>0?true:false;   
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}
