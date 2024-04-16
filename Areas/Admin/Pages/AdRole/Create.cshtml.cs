using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdRole
{
    [Authorize(Roles = "admin")]
    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
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
        public InputModel Input {set; get;}

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if  (!ModelState.IsValid)
            {
                return Page();
            }

            var newRole = new IdentityRole(Input.Name);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"You have just created a new role: {Input.Name}";
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
