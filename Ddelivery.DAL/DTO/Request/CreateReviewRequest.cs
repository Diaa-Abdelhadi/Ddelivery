using System.ComponentModel.DataAnnotations;

namespace Ddelivery.DAL.DTO.Request
{
    public class CreateReviewRequest
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        [MinLength(5)]
        public string Comment { get; set; }
    }
}
