using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
    public class Login
    {
        public string UserNAme { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
