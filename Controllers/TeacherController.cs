using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyUquvMarkaz.Models;

namespace MyUquvMarkaz.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index - List all teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

        // Details - View one teacher's details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // Create - GET
        public IActionResult Create()
        {
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Age,Phone")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // Edit - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Age,Phone")] Teacher teacher)
        {
            if (id != teacher.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // Delete - GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // Delete - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
