using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ecommerce.Authservices
{
    public class AuthModel : IAuthModel
    {
        private readonly UserManager<Appuser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public string TokenValue { get; set; }
        public DateTime ExpirtionvAlue {  get; set; }
        public List<string> RolesValue { get; set; }

        public AuthModel(UserManager<Appuser> userManager , RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }
        public async Task<RoleDTO> AddRole(RoleDTO role)
        {
            var roles = new IdentityRole();
                roles.Name = role.RoleNAme;
                var result = await roleManager.CreateAsync(roles);
                if (result.Succeeded) 
                {
                    return role;
                }
                foreach (var Errors in result.Errors)
                {
                    return new RoleDTO() { 
                        
                        message=$"This Role is already Created" };

                }
                return new RoleDTO();
        }

        public async Task<AuthDTO> Login(Login login)
        {
            var data = new AuthDTO();
            var user = await userManager.FindByNameAsync(login.UserNAme);
            if (user is null)
            {return new AuthDTO { Message = "UserNAme Or Password Is Wrong" };}
            var checkpass = await userManager.CheckPasswordAsync(user,login.Password);
            if (checkpass)
            {
                await JwtsecurityToken(user);
                data.Token = TokenValue;
                data.UserName = login.UserNAme;
                data.ExpiredToken = ExpirtionvAlue;
                data.IsAuthenticated = true;
                data.Message = "Created Succeeded";
                if (user.RefreshTokens.Any(x => x.IsActive))
                {
                    var reftoken = user.RefreshTokens.SingleOrDefault(x => x.IsActive);
                    data.RefToken = reftoken.RefToken;
                    data.ExpiredRefToken = reftoken.ExpiredOn;
                }
                else
                {
                    var reftoken = await GenerateKey();
                    data.RefToken = reftoken.RefToken;
                    data.ExpiredRefToken = reftoken.ExpiredOn;
                    user.RefreshTokens.Add(reftoken);
                    await userManager.UpdateAsync(user);
                }
                return data;
            }

            return new AuthDTO { Message="UserNAme Or Password Is Wrong"};
        }

        public async Task<AuthDTO> RefreshToken(string RefReshToken)
        {
            var data = new AuthDTO();
            var user = await userManager.Users.SingleOrDefaultAsync(x=>x.RefreshTokens.Any(t=>t.RefToken==RefReshToken));
            if (user == null)
            { return new AuthDTO { Message = "Invalid User" }; }
            var reftoken = user.RefreshTokens.SingleOrDefault(x => x.RefToken==RefReshToken);
            if (!reftoken.IsActive)
            { return new AuthDTO { Message = "Invalid User" }; }

            reftoken.RevokedOn= DateTime.Now;
            var refkey = await GenerateKey();
            user.RefreshTokens.Add(refkey);
            await userManager.UpdateAsync(user);
            var newToken = await JwtsecurityToken(user);
            data.Token = TokenValue;
            data.RefToken=refkey.RefToken;
            data.ExpiredToken = ExpirtionvAlue;
            data.ExpiredRefToken = refkey.ExpiredOn;
            data.Roles=RolesValue;
            data.Email = user.Email;
            data.UserName = user.UserName;
            data.IsAuthenticated =true;
            return data;
        }

        public async Task<AuthDTO> Register(RegisterDTO register)
        {
            var data = new AuthDTO();
            var user = new Appuser();
            user.UserName = register.UserNAme;
            user.Email = register.Email;
            user.PasswordHash = register.Password;
            if (await userManager.FindByNameAsync(user.UserName) is not null)
            { return new AuthDTO { Message = "This Name Is already Exist" }; };
            if (await userManager.FindByEmailAsync(user.Email) is not null)
            { return new AuthDTO { Message = "This Email Is already Exist" }; };
            var result = await userManager.CreateAsync(user,user.PasswordHash);
            if (result.Succeeded) 
            {
                await JwtsecurityToken(user);
                data.Token = TokenValue;
                data.Email = register.Email;
                data.UserName = register.UserNAme;
                data.ExpiredToken = ExpirtionvAlue;
                data.IsAuthenticated=true;
                data.Message = "Created Succeeded";
                var reftoken = await GenerateKey();
                data.RefToken = reftoken.RefToken;
                data.ExpiredRefToken = reftoken.ExpiredOn;
                user.RefreshTokens.Add(reftoken);
                await userManager.UpdateAsync(user);
                return data;
            }
            foreach (var item in result.Errors)
            {return new AuthDTO { Message = item.Description };}

            return data;
        }

        public async Task<bool> RevokeToken(string RefReshToken)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.RefToken == RefReshToken));
            if (user == null)
            { return false; }
            var reftoken = user.RefreshTokens.SingleOrDefault(x => x.RefToken == RefReshToken);
            if (!reftoken.IsActive)
            { return false; }

            reftoken.RevokedOn = DateTime.Now;
            await userManager.UpdateAsync(user);
            return true;
        }

        public async Task<JwtSecurityToken> JwtsecurityToken(Appuser user)
        {

            var claim = new List<Claim>();
            claim.Add(new Claim(ClaimTypes.Name, user.UserName));
            claim.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claim.Add(new Claim(JwtHeaderParameterNames.Jwk,Guid.NewGuid().ToString()));
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var sign = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: configuration["jwt:issuer"]
                ,audience: configuration["jwt:audience"]
                ,expires: DateTime.UtcNow.AddHours(1)
                ,claims:claim
                ,signingCredentials:sign);
            TokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            RolesValue = roles.ToList();
            ExpirtionvAlue = DateTime.UtcNow.AddHours(1);

            return token;
        }

        public async Task<RefreshToken> GenerateKey()
        {
            var randNum = new byte[32];
            var generate = new RNGCryptoServiceProvider();
            generate.GetBytes(randNum);
            return new RefreshToken
            {
                RefToken=Convert.ToBase64String(randNum),
                ExpiredOn = DateTime.UtcNow.AddHours(1).ToLocalTime(),
                CreatedOn = DateTime.UtcNow,
            };

        }
    }


}
