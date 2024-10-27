using InterviewManagement.Application.DTOs;
using InterviewManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InterviewManagement.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto userDto)
        {
            var result = await _userService.RegisterUserAsync(userDto);

            if (result == null) // Registration successful
            {
                TempData["Success"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = result;
            return View(userDto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _userService.LoginAsync(loginDto);

            if (token != null && !token.StartsWith("Invalid"))
            {
                TempData["Success"] = "Login successful!";

                // Store JWT token in a cookie (for session management)
                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddHours(1)
                });

                // Redirect to the Dashboard page upon successful login
                return RedirectToAction("Dashboard");
            }

            TempData["Error"] = token;
            return View(loginDto);
        }

        public IActionResult Dashboard()
        {
            // Check if the user is authenticated and has a valid JWT token
            var token = Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "You must log in to access the dashboard.";
                return RedirectToAction("Login");
            }

            return View();
        }


        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            TempData["Success"] = "You have logged out successfully.";
            return RedirectToAction("Login");
        }

    }

}
