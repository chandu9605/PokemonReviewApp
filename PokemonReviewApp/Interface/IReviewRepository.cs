using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IReviewRepository
    {
        Review GetReview(int reviewId);
        ICollection<Review> GetReviews();
        bool ReviewExists(int reviewId);
        ICollection<Review> GetReviewsOfPokemon(int pokeId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool Save();
    }
}
