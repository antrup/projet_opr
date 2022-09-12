using BugTrackerAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerAPI.Data
{
    public class BugTrackerEntities : IdentityDbContext<BugTrackerUser, BugTrackerRole, string, IdentityUserClaim<string>, BugTrackerUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public BugTrackerEntities(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Allow to easily query roles for a specific user
            builder.Entity<BugTrackerUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId);
            });
        }

        //Tickets table
        public DbSet<Ticket> Tickets => Set<Ticket>();

        // Application table
        public DbSet<Application> Applications => Set<Application>();
    }
}
