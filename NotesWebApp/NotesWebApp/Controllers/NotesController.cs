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
            return id != null ? int.Parse(id) : (int?)null;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var notes = await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
            return View(notes);
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
            if (ModelState.IsValid)
            {
                _context.Update(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
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
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                note.IsFavorite = !note.IsFavorite;
                _context.Update(note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }
    }
}
