using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdRole
{
    [Authorize(Roles = "admin")]
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {

        }

        public class InputModel
        {
            [Display(Name = "Name role")]
            [Required(ErrorMessage = "Must enter {0}")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be {2} to {1} characters long")]
            public string Name { get; set; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        public IdentityRole role { get; set; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            if (roleid == null) return NotFound("No role found");

            role = await _roleManager.FindByIdAsync(roleid);
            if (role != null)
            {
                Input = new InputModel()
                {
                    Name = role.Name
                };
                return Page();
            }
            return NotFound("No role found");
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("No role found");
                role = await _roleManager.FindByIdAsync(roleid);

            if (role == null) return NotFound("No role found");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"You have just renamed the role: {Input.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                var uniqueErrors = result.Errors
                    .Select(error => error.Description)
                    .Distinct()
                    .ToList();

                foreach (var error in uniqueErrors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return Page();
        }
    }
}
