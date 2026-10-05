using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager.Data;
using StudentManager.Models.Entities;

namespace StudentManager.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentContext _context;

        public StudentsController(StudentContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View(students);
        }

        public IActionResult Create() => View(new Student());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid) return View(student);

            student.Id = Guid.NewGuid();
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            return student is null ? NotFound() : View(student);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Student student)
        {
            if (id != student.Id) return BadRequest();
            if (!ModelState.IsValid) return View(student);

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            return student is null ? NotFound() : View(student);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student is not null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}