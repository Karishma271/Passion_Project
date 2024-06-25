using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Passion_Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
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

    // Inherits from IdentityDbContext with ApplicationUser as the user type
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        // Factory method to create instances of ApplicationDbContext
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // DbSet properties for interacting with database tables
        public DbSet<Studio> Studios { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        // Optional: Override OnModelCreating to configure entity mappings using Fluent API
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure entity mappings here if needed
            base.OnModelCreating(modelBuilder);
        }
    }
}