﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IReviewerRepository
    {
        bool ReviwerExists(int reviewerId);
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer  reviewer);
        bool Save();
    }
}
