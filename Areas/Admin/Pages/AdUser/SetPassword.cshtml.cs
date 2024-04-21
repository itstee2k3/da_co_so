using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Authorization;

namespace do_an_ltweb.Admin.AdUser;

[Authorize(Roles = "admin")]
public class SetPasswordModel : PageModel
{

    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public SetPasswordModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Must enter {0}")]
        [StringLength(100, ErrorMessage = "{0} must be {2} to {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "Password confirmation is incorrect.")]
        public string ConfirmPassword { get; set; }
    }

    public User user { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound($"No user");
        }

        user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound($"User no found, id = {id}.");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound($"No user");
        }

        user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound($"User no found, id = {id}.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }
         
        await _userManager.RemovePasswordAsync(user);

        var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
        if (!addPasswordResult.Succeeded)
        {
            foreach (var error in addPasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        StatusMessage = $"Just updated the password for the user: {user.UserName}";

        return RedirectToPage("./Index");
    }
}