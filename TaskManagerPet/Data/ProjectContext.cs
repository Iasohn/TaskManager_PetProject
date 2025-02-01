using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagerPet.Models;

namespace TaskManagerPet.Data
{
    public class ProjectContext : IdentityDbContext<User>
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }

        public DbSet<Tasks> Task { get; set; }

        public bool TestConnection()
        {
            return this.Database.CanConnect();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Создаем фиксированные Id для ролей
            var adminRoleId = "1f30e8d1-9a3c-43d4-a672-e94b78fe1f43";
            var userRoleId = "92f99f7e-20b7-48b0-b3de-cf5c73bb5bb6";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                }
            );
        }

    }
}