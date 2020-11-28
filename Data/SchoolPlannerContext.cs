using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore;
using SchoolPlanner.Models;

public class SchoolPlannerContext : DbContext {
    public DbSet<Class> Class { get; set; }
    public DbSet<Classroom> Classroom { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<Subject> Subject { get; set; }
    public DbSet<Lesson> Lesson { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseMySQL("server=localhost;database=NTR20Z;user=marcel;password=cauchy88");
    }

}
