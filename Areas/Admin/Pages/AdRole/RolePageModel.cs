using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdRole
{
    public class RolePageModel : PageModel
    {
          protected readonly RoleManager<IdentityRole> _roleManager;
          protected readonly ApplicationDbContext _context;

          [TempData]
          public string StatusMessage { get; set; }
          public RolePageModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
          {
              _roleManager = roleManager;
              _context = applicationDbContext;
          }
    }
}