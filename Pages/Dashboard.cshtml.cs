using Chatrum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chatrum // page Controller?
{
    [Authorize]
    public class DashboardModel : PageModel // tohle budou zpravy, a nahore textbox na pridani zpravy, tohle pridat to 
    {
        private readonly ApplicationDbContext _context;
        public List<MessagesViewModel> Messages { get; set; }
        public DashboardModel(ApplicationDbContext ApplicationDbContext) {
            _context = ApplicationDbContext;
        }

        // all messages
        public void OnGet() {

        }
        // new message
        public void OnPost() {

        }
    }
}
