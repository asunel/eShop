using eShop.Business.Interfaces;
using eShopService.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopService.Services
{
    public interface IUserService
    {
        Task<string> Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;
        private readonly AppSettings _appSettings;

        public UserService(IUserManager userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            IEnumerable<IdentityUserRole<string>> userRoles;
            var users = await _userManager.GetUsers();
            var user = users.SingleOrDefault(x => x.UserName == username);

            var paswordHasher = new PasswordHasher<IdentityUser>();
            var isPasswordCorrect = paswordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success;

            if (!isPasswordCorrect)
            {
                return null;
            }

            userRoles = await _userManager.GetUserRoles();
            var userRole = userRoles.SingleOrDefault(x => x.UserId == user.Id);

            var roles = await _userManager.GetRoles();
            var role = roles.SingleOrDefault(x => x.Id == userRole.RoleId);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                            new Claim(ClaimTypes.Name, user.Id.ToString()),
                            new Claim(ClaimTypes.Role, role.Name),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenValue = tokenHandler.WriteToken(token);

            // remove password before returning
            //user.Password = null;

            return tokenValue;
        }
    }
}