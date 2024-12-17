using Microsoft.EntityFrameworkCore;

namespace MyUquvMarkaz.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fanlar va talabalar orasidagi ko‘pdan-ko‘p aloqani sozlash
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Students)
                .WithMany(st => st.Subjects);

            // Fanlar va o‘qituvchilar orasidagi birdan-ko‘p aloqani sozlash
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Teacher)
                .WithMany(t => t.Subjects)
                .HasForeignKey(s => s.TeacherId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
