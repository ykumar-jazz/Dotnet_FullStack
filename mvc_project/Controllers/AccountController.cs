using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mvc_project.EMS.Domain.Entities;
using mvc_project.EMS.Infrastructure.Data;
using mvc_project.Models;
using System.Security.Claims;
namespace mvc_project.EMS.Web.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Admin")) //if (User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                return RedirectToAction("Index", "Employee");
            }

            return RedirectToAction("Privacy", "Home");
        }

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(
        string email,
        string password)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == email);
        PasswordHasher<AppUser> hasher = new();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (user == null && result == PasswordVerificationResult.Failed)
        {
            ViewBag.Error = "Invalid Credentials";

            return View();
        }

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Name),
            new (ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults
                .AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults
                .AuthenticationScheme,
            new ClaimsPrincipal(identity));

        if (user.Role == "Admin")
            return RedirectToAction("Index",
                                    "Employee");

        return RedirectToAction("Privacy",
                                "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction("Login");
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
    RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool exists = _context.Users
            .Any(x => x.Email == model.Email);

        if (exists)
        {
            ViewBag.Error = "Email already exists";

            return View(model);
        }

        AppUser user = new()
        {
            Name = model.Name,
            Email = model.Email,
            Role = model.Role,

            // temporary
            PasswordHash = model.Password
        };
        PasswordHasher<AppUser> hasher = new();
        user.PasswordHash = hasher.HashPassword(user, model.Password);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return RedirectToAction("Login");
    }
}
