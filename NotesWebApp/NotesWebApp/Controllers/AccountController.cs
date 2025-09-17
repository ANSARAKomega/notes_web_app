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
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                return RedirectToAction("Dashboard", "Notes");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}