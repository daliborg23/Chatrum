using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chatrum {
    public class UserDbContext : IdentityDbContext<User> {
        DbSet<User> Users { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {

        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder) {
        //    base.OnModelCreating(modelBuilder);
        //    // modelBuilder.Entity<User>().Property(u => u.FullName).IsRequired();
        //    modelBuilder.Entity<User>().Property(u => u.Name).IsRequired();
        //    modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
        //    modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
        //}
    }
}