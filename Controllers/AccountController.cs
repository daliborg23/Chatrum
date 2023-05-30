using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;

namespace Chatrum {
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller {
        private readonly UserDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserDbContext UserDbContext, UserManager<User> userManager, SignInManager<User> signInManager) {
            _context = UserDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel login) { //string username, string password
            var user = await _userManager.FindByNameAsync(login.Name);

            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password)) {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, user.UserName)
                // Add additional claims as needed
                };
                SecretKey sc = new SecretKey();
                
                var token = GenerateJwtToken(SecretKey._secretKey, "Chatrum-Issuer", "audience", 10, claims);

                // Use the SignInManager to sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                return Ok(new { Token = token });
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return BadRequest("Invalid username or password");


            //var claims = new[] { new Claim(ClaimTypes.Name, login.Name) };
            // if identity exists - You are already logged in?
            // vrati jen zpravu, redirect tady, nebo pak? a jak to pozna ze je nekdo autorizovany? prece na tu stranku pujde jit
            // i bez loginu?
            //var token = GenerateJwtToken(SecretKey._secretKey, "Chatrum-Issuer","audience",10,claims);
            //prirazeni do UserTokens tabulky?
            //return Ok(new { Token = token });
            //_context.UserTokens.Add(token);
            //_context.SaveChanges();

            //return Ok(new { Token = token });
            //return Ok("Login successful.");

            ////
            string GenerateJwtToken(string secretKey, string issuer, string audience, int expiryMinutes, Claim[] claims) {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                    signingCredentials: credentials
                );
                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.WriteToken(token);
            }
        }
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout() {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the homepage or a public page
            return Ok("Logout.");
        }

        //private bool IsValidUser(string username, string password) {
        //    // Validate the user's credentials against the database or other storage
        //    // Return true if the credentials are valid; otherwise, return false
        //    // You can use a membership provider, Identity framework, or custom logic for validation
        //    // This is just an example, you should replace it with your own validation logic
            
        //    if (_context.Users.Where(x => x.Name == username && x.Password == password).Any()) return true;
        //    return false;
        //}

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                // Create a new user based on the registration form data
                var user = new User {
                    Name = model.Name,
                    UserName = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    RegistrationTime = DateTime.Now,
                    
                };

                // Use the UserManager to create the user
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) {
                    // Optionally, you can sign in the user after registration
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    //_context.Users.Add(user);
                    //_context.SaveChanges();
                    // Redirect to a protected page or the homepage
                    // return Ok("Registration successful.")
                    return Ok(model);
                }

                // If registration failed, add the errors to the ModelState
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(result);
            }

            // If there are validation errors, show the registration form with error messages
            return BadRequest(model);
        }
        //[HttpPut("ChangeProfile")]
        //public IEnumerable<User> ChangeProfileSettings(string? name) {

        //}
        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers(string? id) { 
            if (string.IsNullOrEmpty(id)) return _context.Users.ToList();
            return _context.Users.Where(x => x.Id.ToString() == id);
        }
    }
}