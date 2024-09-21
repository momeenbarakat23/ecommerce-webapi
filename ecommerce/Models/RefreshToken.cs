using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    [Owned]
    public class RefreshToken
    {
        public string RefToken { get; set; }

        public DateTime ExpiredOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime? RevokedOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiredOn;
        public bool IsActive => RevokedOn == null && !IsExpired; 

    }
}
