//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using do_an_ltweb.Models;

//namespace do_an_ltweb.Admin.AdProduct
//{
//    // Policy: Tao ra cac policy -> AllowEditRole
//    [Authorize(Roles = "admin")]
//    public class EditModel : RolePageModel
//    {
//        public EditModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
//        {

//        }

//        public class InputModel
//        {
//            [Display(Name = "Tên của role")]
//            [Required(ErrorMessage = "Phải nhập {0}")]
//            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài {2} đến {1} ký tự")]
//            public string Name { get; set; }
//        }

//        [BindProperty]
//        public InputModel Input { set; get; }

//        public IdentityRole role { get; set; }

//        public async Task<IActionResult> OnGet(string roleid)
//        {
//            if (roleid == null) return NotFound("Không tìm thấy role");

//            role = await _roleManager.FindByIdAsync(roleid);
//            if (role != null)
//            {
//                Input = new InputModel()
//                {
//                    Name = role.Name
//                };
//                return Page();
//            }
//            return NotFound("Không tìm thấy role");
//        }

//        public async Task<IActionResult> OnPostAsync(string roleid)
//        {
//            if (roleid == null) return NotFound("Không tìm thấy role");
//            role = await _roleManager.FindByIdAsync(roleid);
//            if (role == null) return NotFound("Không tìm thấy role");
//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            role.Name = Input.Name;
//            var result = await _roleManager.UpdateAsync(role);

//            if (result.Succeeded)
//            {
//                StatusMessage = $"Bạn vừa đổi tên: {Input.Name}";
//                return RedirectToPage("./Index");
//            }
//            else
//            {
//                result.Errors.ToList().ForEach(error => {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                });
//            }
//            return Page();
//        }
//    }
//}
