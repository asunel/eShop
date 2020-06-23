using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetUsers();
        Task<IEnumerable<IdentityUserRole<string>>> GetUserRoles();
        Task<IEnumerable<IdentityRole>> GetRoles();
    }
}
