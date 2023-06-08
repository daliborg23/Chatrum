using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chatrum // page Controller?
{
    [Authorize]
    public class DashboardModel : PageModel // tohle budou zpravy, a nahore textbox na pridani zpravy, tohle pridat to 
    {
        public string Name { get; set; }
        public string UserList { get; set; }
        private readonly UserDbContext _context;
        public List<User> UsersTable { get; set; }
        public DashboardModel(UserDbContext UserDbContext) {
            _context = UserDbContext;
        }

        public void OnGet() {
            Name = User.Identity.Name;
            UserList = "Registered users: ";

            ViewData["name"] = Name;
            UsersTable = _context.Users.ToList();
            List<User> users = _context.Users.ToList();
            foreach (var user in users) {
                UserList += user.Name+", ";
            }
            //ViewData[UserList] = UserList;
        }
    }
}
