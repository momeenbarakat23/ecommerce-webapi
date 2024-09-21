using Microsoft.AspNetCore.Authorization;

namespace ecommerce.DTO
{
    public class RoleDTO
    {
        public string RoleNAme { get; set; }
        
        public string? message { get; set; }
    }
}
