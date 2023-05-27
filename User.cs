using Microsoft.AspNetCore.Identity;

namespace Chatrum {
    public class User : IdentityUser {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime? RegistrationTime { get; set; }
    }
}
