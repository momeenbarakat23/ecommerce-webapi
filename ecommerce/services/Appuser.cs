using ecommerce.Models;
using Microsoft.AspNetCore.Identity;

namespace ecommerce.services
{
    public class Appuser : IdentityUser
    {
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
