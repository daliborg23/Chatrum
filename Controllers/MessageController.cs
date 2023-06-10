using Chatrum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chatrum.Controllers {
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MessageController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UsersViewModel> _userManager;
        private readonly SignInManager<UsersViewModel> _signInManager;

        public MessageController(ApplicationDbContext ApplicationDbContext, UserManager<UsersViewModel> userManager, SignInManager<UsersViewModel> signInManager) {
            _context = ApplicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet("Messages")]
        public async Task<IActionResult> Messages(string? id) {
            List<MessagesViewModel> messages = new List<MessagesViewModel>();
            var dbMessages = _context.Messages.ToList();
            foreach (var dbMessage in dbMessages) {
                MessagesViewModel messageViewModel = new MessagesViewModel {
                    Id = dbMessage.Id,
                };
                messages.Add(messageViewModel);
            }
            return Ok(messages);
        }
        [HttpPost("NewMessage")]
        public async Task<IActionResult> NewMessage(MessagesViewModel model) {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid) {
                MessagesViewModel message = new MessagesViewModel {
                    //Id = Guid.NewGuid().ToString(),
                    //PostBy = user.Id,
                    PostBy = "0000000-0000-0000-0000-00000000000",
                    Message = model.Message
                };
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            return BadRequest(ModelState);
        }
    }
}
