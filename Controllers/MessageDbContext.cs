using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chatrum {
	public class MessageDbContext : IdentityDbContext<User> {
		DbSet<User> Messages { get; set; }
		public MessageDbContext(DbContextOptions<UserDbContext> options) : base(options) {

		}
	}
}
