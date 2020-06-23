using eShop.Business.Interfaces;
using eShop.Common.DTO;
using eShopService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace eShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUserService _userService;
        private readonly IUserManager _userManager;

        public UsersController(IProductManager productManager
            ,ILogger<ProductsController> logger
            , IUserService userService, IUserManager userManager)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]User userParam)
        {
            var token = await _userService.Authenticate(userParam.Username, userParam.Password);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(token);
        }
    }
}
