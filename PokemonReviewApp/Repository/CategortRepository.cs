using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategortRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategortRepository(DataContext context)
        {
            _context = context;
        }

        public bool CareateCategory(Category category)
        {
            _context.Add(category);
            return Save();

        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
            
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategories(int categotyId)
        {
            return _context.PokemonCategories.Where(e=>e.CategoryId==categotyId).Select(c=>c.Pokemon).ToList();
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved>0?true:false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
