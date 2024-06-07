﻿using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class Reviewerrepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public Reviewerrepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers.Where(rv => rv.Id == reviewerId).Include(e=>e.Reviews).FirstOrDefault();

        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _context.Reviews.Where(rr=>rr.Reviewer.Id==reviewerId).ToList(); 
        }

        public bool ReviwerExists(int reviewerId)
        {
            return _context.Reviewers.Any(r=>r.Id==reviewerId);
        }

        public bool Save()
        {
           var save=_context.SaveChanges();
            return save>0?true:false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}
