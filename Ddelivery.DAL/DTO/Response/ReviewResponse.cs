using System;

namespace Ddelivery.DAL.DTO.Response
{
    public class ReviewResponse
    {
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
    }
}
