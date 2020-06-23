using eShop.Business.Interfaces;
using eShop.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Business.Manager
{
    public class UserManager: IUserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<IdentityUser>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<IEnumerable<IdentityUserRole<string>>> GetUserRoles()
        {
            return await _userRepository.GetUserRoles();
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await _userRepository.GetRoles();
;        }
    }
}
