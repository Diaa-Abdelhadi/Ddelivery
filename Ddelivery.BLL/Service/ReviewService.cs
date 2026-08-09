using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using Ddelivery.DAL.Models;
using Ddelivery.DAL.Repository;
using Mapster;

namespace Ddelivery.BLL.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IOrderRepository orderRepository, IReviewRepository reviewRepository)
        {
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }
        public async Task<BaseResponse> AddReviewAsync(string userId, CreateReviewRequest request, int mealId)
        {
            var hasDelivered = await _orderRepository.HasUserDeliveredOrderForMeal(userId, mealId);
            if (!hasDelivered)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You can only review meals you have purchased and received."
                };
            }
            var alreadyReviewed = await _reviewRepository.HasUserReviewMeal(userId, mealId);
            if (alreadyReviewed)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You have already reviewed this meal."
                };
            }
            var review = request.Adapt<Review>();
            review.UserId = userId;
            review.MealId = mealId;
            await _reviewRepository.AddReview(review);

            return new BaseResponse
            {
                Success = true,
                Message = "Review added successfully."
            };
        }
    }
}
