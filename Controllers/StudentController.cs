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

        // READ : liste
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View(students);
        }

        // CREATE
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid) return View(student);

            student.Id = Guid.NewGuid();
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // UPDATE
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            return student is null ? NotFound() : View(student);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Student student)
        {
            if (id != student.Id) return