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

namespace do_an_ltweb.Admin.AdCategory.AdCategoryMaterial
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public List<CategoryMaterial> CategoryMaterials { get; set; }


        public async Task OnGet()
        {
            CategoryMaterials = await _context.CategoryMaterials.OrderBy(p => p.IdCategory).ToListAsync();
        }
        public void OnPost() => RedirectToPage();
    }
}
