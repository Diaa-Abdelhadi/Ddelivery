using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public interface IReviewService
    {
        Task<BaseResponse> AddReviewAsync(string userId, CreateReviewRequest request, int mealId);
    }
}
