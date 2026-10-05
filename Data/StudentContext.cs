using Microsoft.EntityFrameworkCore;
using StudentManager.Models.Entities;

namespace StudentManager.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}
