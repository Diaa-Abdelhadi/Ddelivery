using Ddelivery.DAL.Data;
using Ddelivery.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> HasUserReviewMeal(string userId, int mealId)
        {
            return await _context.Reviews.AnyAsync(r => r.UserId == userId && r.MealId == mealId);
        }
        public async Task<Review> AddReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }
    }
}
