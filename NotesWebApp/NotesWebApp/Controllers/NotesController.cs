using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesWebApp.Migrations;
using NotesWebApp.Models;

namespace NotesWebApp.Controllers
{
    public class NotesController : Controller
    {
        private readonly NotesDbContext _context;

        public NotesController(NotesDbContext context)
        {
            _context = context;
        }

        private int? GetUserId()
        {
            var id = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(id)) return null;

            return int.TryParse(id, out var userId) ? userId : (int?)null;
        }


        public async Task<IActionResult> Dashboard(bool favorites = false)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var notes = _context.Notes.Where(n => n.UserId == userId.Value);
            if (favorites)
                notes = notes.Where(n => n.IsFavorite);

            ViewBag.FavoritesOnly = favorites;
            return View(await notes.ToListAsync());
        }



        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Notes note)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            note.UserId = userId.Value;
            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(note);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            return note == null ? NotFound() : View(note);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Notes note)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                note.UserId = userId.Value;

                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Dashboard");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.InnerException?.Message);
                    ModelState.AddModelError("", "Error saving note.");
                }
            }
            return View(note);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int id, bool? favorites)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId.Value);
            if (note != null)
            {
                note.IsFavorite = !note.IsFavorite;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Dashboard", new { favorites = favorites ?? false });
        }

    }
}
