using Microsoft.EntityFrameworkCore;
using SchoolPlanner.Models;

public class SchoolPlannerContext : DbContext {
    public DbSet<Class> Class { get; set; }
    public DbSet<Classroom> Classroom { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<Subject> Subject { get; set; }
    public DbSet<Lesson> Lesson { get; set; }

    public SchoolPlannerContext () : base() {}
    public SchoolPlannerContext (DbContextOptions<SchoolPlannerContext> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseMySQL("server=localhost;database=NTR20Z;user=marcel;password=cauchy88");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Class>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<Classroom>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Number).IsUnique();
            });

            modelBuilder.Entity<Teacher>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Surname).IsUnique();
            });

            modelBuilder.Entity<Subject>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<Lesson>(entity => {
                entity.HasKey(e => e.Id);
            });
        }
}