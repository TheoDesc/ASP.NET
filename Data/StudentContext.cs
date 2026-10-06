using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManager.Models.Entities;

namespace StudentManager.Data
{
    public class StudentContext : IdentityDbContext<ApplicationUser>
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}