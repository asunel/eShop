using eShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                var isLoginSuccess = await LoginUser(user, loginViewModel.Password);
                if (isLoginSuccess)
                {
                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                    {
                        return RedirectToAction("List", "Product");
                    }

                    return Redirect(loginViewModel.ReturnUrl);
                }
            }

            ModelState.AddModelError("", "Username/password not found");
            return View(loginViewModel);
        }

        public async Task<bool> LoginUser(IdentityUser user, string password) {
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            return result.Succeeded;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

                if (user != null)
                {
                    ModelState.AddModelError("", $"Username: {loginViewModel.UserName} already exists!");
                }
                else
                {

                    user = new IdentityUser() { UserName = loginViewModel.UserName };
                    var result = await _userManager.CreateAsync(user, loginViewModel.Password);

                    if (result.Succeeded)
                    {
                        var isLoginSuccess = await LoginUser(user, loginViewModel.Password);
                        if (isLoginSuccess)
                        {
                            if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                            {
                                return RedirectToAction("List", "Product");
                            }

                            return Redirect(loginViewModel.ReturnUrl);
                        }
                    }
                    else
                    {
                        var error = string.Join("", result.Errors.ToList().Select(e => e.Description).ToList());
                        ModelState.AddModelError("", error);
                    }
                }
            }

            return View(loginViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(actionName: "List", controllerName: "Product");
        }
    }
}
