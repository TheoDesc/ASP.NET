using Microsoft.AspNetCore.Mvc;
using StudentManager.Models.Entities;
using StudentManager.Services;

namespace StudentManager.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _studentService.GetAllAsync());
        }

        public IActionResult Create() => View(new Student());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid) return View(student);

            await _studentService.CreateAsync(student);
            TempData["Success"] = $"{student.Name} a été ajouté.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);
            return student is null ? NotFound() : View(student);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Student student)
        {
            if (id != student.Id) return BadRequest();
            if (!ModelState.IsValid) return View(student);

            await _studentService.UpdateAsync(student);
            TempData["Success"] = $"{student.Name} a été modifié.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student is null) return NotFound();

            var name = student.Name;
            await _studentService.DeleteAsync(id);
            TempData["Success"] = $"{name} a été supprimé.";
            return RedirectToAction(nameof(Index));
        }
    }
}
