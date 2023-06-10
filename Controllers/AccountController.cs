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
using Microsoft.AspNetCore.Authorization;

namespace Chatrum {
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UsersViewModel> _userManager;
        private readonly SignInManager<UsersViewModel> _signInManager;

        public AccountController(ApplicationDbContext ApplicationDbContext, UserManager<UsersViewModel> userManager, SignInManager<UsersViewModel> signInManager) {
            _context = ApplicationDbContext;
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
                };
                SecretKey sc = new SecretKey();
                
                var token = GenerateJwtToken(SecretKey._secretKey, "Chatrum-Issuer", "audience", 10, claims);

                await _signInManager.SignInAsync(user, isPersistent: false);

                return Ok(new { Token = token });
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return BadRequest("Invalid username or password");

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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logout.");
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                var user = new UsersViewModel {
                    Name = model.Name,
                    UserName = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    RegistrationTime = DateTime.Now,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(model);
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(result);
            }
            return BadRequest(model);
        }
        //[HttpPut("ChangeProfile")]
        //public IEnumerable<User> ChangeProfileSettings(string? name) {

        //}

        // jinde?
        [Authorize]
        [HttpGet("GetUsers")]
        public IEnumerable<UsersViewModel> GetUsers(string? id) { 
            if (string.IsNullOrEmpty(id)) return _context.Users.ToList();
            return _context.Users.Where(x => x.Id.ToString() == id);
        }
    }
}