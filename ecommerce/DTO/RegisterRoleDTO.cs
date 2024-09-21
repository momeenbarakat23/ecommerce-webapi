using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
    public class RegisterRoleDTO
    {
        public string UserNAme { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public List<string> Roles { get; set; }
    }
}
