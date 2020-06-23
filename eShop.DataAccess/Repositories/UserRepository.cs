using eShop.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.DataAccess.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly DatabaseContext _ctx;
        public UserRepository(DatabaseContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<IdentityUser>> GetUsers()
        {
            return await _ctx.Users.ToListAsync();
        }

        public async Task<IEnumerable<IdentityUserRole<string>>> GetUserRoles()
        {
            return await _ctx.UserRoles.ToListAsync();
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await _ctx.Roles.ToListAsync();
        }
    }
}
