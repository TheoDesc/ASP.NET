using Microsoft.EntityFrameworkCore;
using StudentManager.Data;
using StudentManager.Models.Entities;

namespace StudentManager.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentContext _context;

        public StudentService(StudentContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task CreateAsync(Student student)
        {
            student.Id = Guid.NewGuid();
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student is not null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
    }
}