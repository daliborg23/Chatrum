using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chatrum // page Controller?
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        public string Name { get; set; }
        public string UserList { get; set; }
        private readonly UserDbContext _context;
        public DashboardModel(UserDbContext UserDbContext) {
            _context = UserDbContext;
        }

        public void OnGet() {
            Name = User.Identity.Name;
            UserList = "Registered users: ";

            ViewData["name"] = Name;

            List<User> users = _context.Users.ToList();
            foreach (var user in users) {
                UserList += user.Name+", ";
            }
            //ViewData[UserList] = UserList;
        }
    }
}
