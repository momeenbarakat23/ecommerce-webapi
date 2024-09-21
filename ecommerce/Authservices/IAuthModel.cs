using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Authservices
{
    public interface IAuthModel
    {
        public Task<RoleDTO> AddRole(RoleDTO role);
        public Task<AuthDTO> Register(RegisterDTO register);
        public Task<AuthDTO> Login(Login login);
        public Task<AuthDTO> RefreshToken(string RefReshToken);
        public Task<bool> RevokeToken(string RefReshToken);
    }
}
