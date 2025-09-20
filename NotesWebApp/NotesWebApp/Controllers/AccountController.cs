using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesWebApp.Models;

namespace NotesWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly NotesDbContext _context;

        public AccountController(NotesDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(Users model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);

                    return RedirectToAction("Dashboard", "Notes");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }



        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(Users user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }



        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }


    }
}