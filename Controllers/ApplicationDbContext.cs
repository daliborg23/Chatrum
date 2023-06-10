using Chatrum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chatrum {
    public class ApplicationDbContext : IdentityDbContext<UsersViewModel> {
        public DbSet<UsersViewModel> Users { get; set; }
        public DbSet<MessagesViewModel> Messages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }
    }
}