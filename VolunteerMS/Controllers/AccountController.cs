using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VolunteerMS.Data;
using VolunteerMS.Models;

namespace VolunteerMS.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _pwHasher;

        public AccountController(AppDbContext context)
        {
            _context = context;
            _pwHasher = new PasswordHasher<User>();
        }
        
        /// Returns the login page for the user to enter their credentials.
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Sets the model state error, keep it vauge 
        /// </summary>
        private void DenyAccess()
        {
            ModelState.AddModelError("", "Invalid username or password.");
            //can do more here to deny access if needed.
        }

        /// <summary>
        /// Claim the user ident, sets a cookie for the user login with identifying information, and returns a redirect to the home page.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<IActionResult> ClaimUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            return RedirectToAction("Index", "Home");
        }

        // Logs in the user and validates them. Gives back a cookie to their system to allow them to access other pages in the system.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);


            // is invalid user?
            // user was null or password hashing mismatch
            if (user == null || _pwHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                // kickback to login
                DenyAccess();
                return View();
            }

           
            return await ClaimUser(user);
        }
    }
}
