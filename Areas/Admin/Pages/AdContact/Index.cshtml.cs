using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using do_an.Models;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdContact
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public List<Contact> Contacts { get; set; }

        public async Task OnGet()
        {
            Contacts = await _context.Contacts.ToListAsync<Contact>();
        }
        public void OnPost() => RedirectToPage();
    }
}
