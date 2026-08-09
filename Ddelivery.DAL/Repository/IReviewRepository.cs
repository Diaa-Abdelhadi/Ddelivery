using Ddelivery.DAL.Models;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface IReviewRepository
    {
        Task<bool> HasUserReviewMeal(string userId, int mealId);
        Task<Review> AddReview(Review review);
    }
}
