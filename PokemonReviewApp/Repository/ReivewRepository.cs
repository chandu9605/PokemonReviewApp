﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReivewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReivewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfPokemon(int pokeId)
        {
            return _context.Reviews.Where(p=>p.Pokemon.Id==pokeId).ToList();
           
        }

        public bool ReviewExists(int reviewId)
        {
            return _context.Reviews.Any(r=>r.Id==reviewId);
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save>0?true:false;
        }

        public bool UpdateReview(Review review)
        {
           _context.Update(review);
            return Save();
        }
    }
}
