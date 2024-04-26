using Microsoft.EntityFrameworkCore;
using StudentPortal.API.Models;

namespace StudentPortal.API.Data
{
    public class StudentDbContext: DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options): base(options)
        {
            
        }
        public DbSet<StudentModel> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentModel>().HasKey(s => s.RollNumber);
        }
    }
}
