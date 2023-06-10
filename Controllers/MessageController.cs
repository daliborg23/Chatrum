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
            if (string.IsNullOrEmpty(id)) return (IActionResult)_context.Messages.ToList();
            return (IActionResult)_context.Messages.Where(x => x.Id.ToString() == id);
        }
        [HttpPost("NewMessage")]
        public async Task<IActionResult> NewMessage(MessagesViewModel model) {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid) {
                var message = new MessagesViewModel {
                   PostBy = user.Id,
                   Message = model.Message
                };
                await _context.AddAsync(message);
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
            return BadRequest(model);
        }
    }
}
