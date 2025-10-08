using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await _accountService.AuthenticateAsync(model.Email, model.Password);
			if (user == null)
			{
				ModelState.AddModelError("", "Invalid email or password.");
				return View(model);
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.FullName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.IsAdmin? "Admin" : "User")
			};

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authProperties = new AuthenticationProperties
			{
				IsPersistent = model.RememberMe
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register(SignUpViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = new User
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				IsAdmin = false,
				IsActive = true
			};

			var result = await _accountService.RegisterAsync(user, model.Password);
			if (!result)
			{
				ModelState.AddModelError("Email", "A user with this email address already exists.");
				return View(model);
			}

			// Show success message and redirect to login
			TempData["SuccessMessage"] = "Registration successful! Please log in with your credentials.";
			return RedirectToAction("Login");
		}

		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}
	}
}
