using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using debug_failure.Areas.Identity.Data;

namespace debug_failure.Data
{
    public class ApplicationDataContext : IdentityDbContext<User>
    {
        // A recommended constructor overload when using EF Core
        // with dependency injection.
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(user => user.UserData)
                .WithOne(ud => ud.Owner);
        }

        public DbSet<UserData> UserData { get; set; }
    }
}
