using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdUser
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        public IndexModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public class UserAndRole : User
        {
            public string RoleNames { get; set; }
        }
        public List<UserAndRole> Users { set; get; }

        public async Task OnGet()
        {
            var users = await _userManager.Users.ToListAsync();
            Users = new List<UserAndRole>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userAndRole = new UserAndRole
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    LockoutEnd = user.LockoutEnd,
                    RoleNames = string.Join(",", roles)
                };
                Users.Add(userAndRole);
            }

        }

        public void OnPost() => RedirectToPage();
    }
}
