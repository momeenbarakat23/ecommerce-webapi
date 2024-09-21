using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
    public class RegisterDTO
    {
        public string UserNAme { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
