using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DynamicRoleBasedAccess.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<RoleAccess> RoleAccesses { get; set; }
    }

    public class RoleAccess
    {
        public int Id { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string RoleId { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<RoleAccess> RoleAccesses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleAccess>().Property(ra => ra.Action)
                .IsUnicode(false).HasMaxLength(70).IsRequired();
            modelBuilder.Entity<RoleAccess>().Property(ra => ra.Controller)
                .IsUnicode(false).HasMaxLength(70).IsRequired();
            modelBuilder.Entity<RoleAccess>().HasRequired(ra => ra.Role)
                .WithMany(r => r.RoleAccesses)
                .HasForeignKey(ra => ra.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}