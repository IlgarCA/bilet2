using BiletIki.Models;
using Microsoft.EntityFrameworkCore;

namespace BiletIki.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Employee>().HasKey(p => p.Id);
        //    base.OnModelCreating(modelBuilder);
        //}
        public DbSet<Employee> Employees { get; set; }
    }
}
