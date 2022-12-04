using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MusiCom.Core.Contracts;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data.Entities;
using MusiCom.Models.User;

namespace MusiCom.Controllers
{
    /// <summary>
    /// Contains Register, Login and Logout functionalities
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        /// <summary>
        /// Renders view for Registration
        /// </summary>
        /// <returns>View for Registration if not authenticated, otherwise redirects to Home</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new RegisterViewModel();

            return View(model);
        }

        /// <summary>
        /// Registers the User into the system
        /// </summary>
        /// <param name="model">Holds the User data for registration</param>
        /// <returns>Redirects to Login if data is valid, if not, returns the model for another register attempt</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                FirstName= model.FirstName,
                LastName= model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        /// <summary>
        /// Renders view for Login
        /// </summary>
        /// <returns>View for Login if not authenticated, otherwise redirects to Home</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel();

            return View(model);
        }

        /// <summary>
        /// Signs in the User
        /// </summary>
        /// <param name="model">Holds the User data for login</param>
        /// <returns>Redirects to Home if data is valid, if not, returns the model for another login attempt</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.UserName);

            if (user.IsDeleted) 
            {
                ModelState.AddModelError("", "Such account no longer exists.");
                return View(model);
            }

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid Login!");

            return View(model);
        }

        /// <summary>
        /// Signs out the User
        /// </summary>
        /// <returns>Redirects to Home</returns>
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            return View(user);
        }

        /// <summary>
        /// Changes or Adds Photo to the User Profile
        /// </summary>
        /// <param name="Photo">Photo file passed by the View</param>
        /// <returns>Redirects to User Details page</returns>
        [HttpPost]
        public async Task<IActionResult> ChangeOrAddPhoto(IFormFile image)
        {
            if (image == null)
            {
                return RedirectToAction("Details");
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            string type = image.ContentType;

            if (!type.Contains("image"))
            {
                throw new InvalidOperationException();
            }

            string contentType = type.Substring(type.IndexOf('/') + 1, type.Length - type.Substring(0, type.IndexOf('/')).Length - 1);

            if (contentType != "png" && contentType != "jpeg" && contentType != "jpg")
            {
                throw new InvalidOperationException("Please import an image in one of the formats shown above!");
            }

            //TODO: Fix
            if (image.Length > 0)
            {
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                user.Image = stream.ToArray();
            }
            else
            {
                throw new InvalidOperationException();
            }

            await userManager.UpdateAsync(user);

            return RedirectToAction("Details");
        }

        /// <summary>
        /// Deletes a Photo from the User profile if it exists
        /// </summary>
        /// <returns>Redirects to User Details page</returns>
        public async Task<IActionResult> DeletePhoto()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            user.Image = null;

            await userManager.UpdateAsync(user);

            return RedirectToAction("Details");
        }
    }
}
