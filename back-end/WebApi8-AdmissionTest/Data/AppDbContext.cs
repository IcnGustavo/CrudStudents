using Microsoft.EntityFrameworkCore;
using WebApi8_AdmissionTest.Models;

namespace WebApi8_AdmissionTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<StudentModel> Students { get; set; }
    }
}
