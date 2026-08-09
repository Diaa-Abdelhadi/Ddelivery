using System.Collections.Generic;

namespace Ddelivery.DAL.DTO.Response
{
    public class UserListResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public List<string> Roles { get; set; }
    }
}
