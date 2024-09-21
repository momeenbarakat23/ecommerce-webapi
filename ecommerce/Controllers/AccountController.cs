using ecommerce.Authservices;
using ecommerce.DTO;
using ecommerce.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthModel authModel;
        private readonly UserManager<Appuser> userManager;

        public AccountController(IAuthModel authModel,UserManager<Appuser> userManager)
        {
            this.authModel = authModel;
            this.userManager = userManager;
        }

        [HttpPost("addRole")]
        public async Task<IActionResult> AddRole(RoleDTO role)
        {
            var result = await authModel.AddRole(role);
            if (result.RoleNAme !=null)
            {
                return Ok(result.RoleNAme);
            }
            return BadRequest(result.message =$" Role {role.RoleNAme} Is Already Created");
        }
        [HttpPost("Registerwithrole")]
        public async Task<IActionResult> Registerwithrole(RegisterRoleDTO  registerRole)
        {
            if (ModelState.IsValid)
            {
                var register = new RegisterDTO();
                register.UserNAme = registerRole.UserNAme;
                register.Password = registerRole.Password;
                register.Email = registerRole.Email;
                var user = await authModel.Register(register);
             
                    user.Roles= registerRole.Roles;

                return Ok(user);
            }
            return BadRequest(ModelState);

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                var user = await authModel.Register(register);
                if (user.RefToken is not null)
                { setinCookie(user.RefToken); }
                return Ok(user);
            }
            return BadRequest(ModelState);

        }

        [HttpPost("login")]
        public async Task<IActionResult> login(Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await authModel.Login(login);
                if (user.RefToken is not null)
                {setinCookie(user.RefToken);}
                return Ok(user);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            if (ModelState.IsValid)
            {
                var reftoken = Request.Cookies["RefreshToken"];
                var result = await authModel.RefreshToken(reftoken);
                if (result.IsAuthenticated)
                {
                    setinCookie(result.RefToken);
                    return Ok(result);
                }
            }
            return BadRequest();
        }
        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken()
        {
            if (ModelState.IsValid)
            {
                var reftoken = Request.Cookies["RefreshToken"];
                var result = await authModel.RevokeToken(reftoken);
                if (result)
                {
                    return Ok(result);
                }
            }
            return BadRequest("Invalid Token");
        }

        private void setinCookie(string RefreshToken)
        {
            var CookieOp = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1),
            };

            Response.Cookies.Append("RefreshToken", RefreshToken,CookieOp);
        }
    }
}
