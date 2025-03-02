using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PatiliDost.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PatiliDost.Data
{
    public class PatiAPIDbContext : IdentityDbContext<AppUser>
    {
        public PatiAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<ServicePT> Services { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Login> Logins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

   
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(r => new { r.UserId, r.RoleId });

         
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Moderator", NormalizedName = "MODERATOR" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" }
            );
        }
    }
}
