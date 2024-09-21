using System.Text.Json.Serialization;

namespace ecommerce.DTO
{
    public class AuthDTO
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ExpiredToken { get; set; }
        [JsonIgnore]
        public string RefToken { get; set; }
        public DateTime ExpiredRefToken { get; set; }

    }
}
