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

namespace do_an_ltweb.Admin.Role
{
    [Authorize(Policy = "AllowEditRole")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {
        }

        public class RoleModel : IdentityRole
        {
            public string[] Claims { get; set; }
        }
        public List<RoleModel> roles  {set; get;}

        public async Task OnGet()
        {
            // _roleManager.GetClaimsAsync()
           var r = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
           roles = new List<RoleModel>();
           foreach (var _r in r)
           {
               var claims = await _roleManager.GetClaimsAsync(_r);
               var claimsString = claims.Select(c => c.Type  + "=" + c.Value);

               var rm = new RoleModel()
               {
                   Name = _r.Name,
                   Id = _r.Id,
                   Claims = claimsString.ToArray()
               };
               roles.Add(rm);
           }


        }

        public void OnPost() => RedirectToPage();
    }
}
