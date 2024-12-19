using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyUquvMarkaz.Models;

namespace MyUquvMarkaz.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Teacher)
                .ToListAsync();
            return View(subjects);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var subject = await _context.Subjects
                .Include(s => s.Teacher)
                .Include(s => s.Students)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subject == null) return NotFound();

            return View(subject);
        }

        public IActionResult Create()
        {
            ViewBag.Teachers = new SelectList(_context.Teachers, "Id", "FullName");
            ViewBag.Students = _context.Students.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject, List<int> SelectedStudentIds)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Subjects.Add(subject);
                    await _context.SaveChangesAsync();

                    foreach (var studentId in SelectedStudentIds)
                    {
                        var student = await _context.Students.FindAsync(studentId);
                        if (student != null)
                        {
                            student.Subjects ??= new List<Subject>();
                            student.Subjects.Add(subject);
                        }
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            ViewBag.Teachers = new SelectList(_context.Teachers, "Id", "FullName", subject.TeacherId);
            ViewBag.Students = _context.Students.ToList();
            return View(subject);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var subject = await _context.Subjects.Include(s => s.Students).FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null) return NotFound();

            ViewBag.Teachers = new SelectList(_context.Teachers, "Id", "FullName", subject.TeacherId);
            ViewBag.Students = _context.Students.ToList(); 
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject subject, List<int> SelectedStudentIds)
        {
            if (id != subject.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);

                    var currentStudents = _context.Subjects
                        .Include(s => s.Students)
                        .FirstOrDefault(s => s.Id == id)?.Students;

                    if (currentStudents != null)
                    {
                        foreach (var currentStudent in currentStudents)
                        {
                            currentStudent.Subjects.Remove(subject);
                        }
                    }

                    foreach (var studentId in SelectedStudentIds)
                    {
                        var student = await _context.Students.FindAsync(studentId);
                        if (student != null)
                        {
                            student.Subjects ??= new List<Subject>();
                            student.Subjects.Add(subject);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id)) return NotFound();
                    else throw;
                }
            }

            ViewBag.Teachers = new SelectList(_context.Teachers, "Id", "FullName", subject.TeacherId);
            ViewBag.Students = _context.Students.ToList();
            return View(subject);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var subject = await _context.Subjects
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subject == null) return NotFound();

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
