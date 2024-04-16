using System.Threading.Tasks;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an.Models;

namespace do_an_ltweb.Admin.AdContact
{
    public class DeleteContactModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteContactModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Contact Contact { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? contactId)
        {
            if (contactId == null)
            {
                return NotFound();
            }

            Contact = await _context.Contacts.FindAsync(contactId);

            if (Contact == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? contactId)
        {
            if (contactId == null)
            {
                return NotFound();
            }

            Contact = await _context.Contacts.FindAsync(contactId);

            if (Contact != null)
            {
                _context.Contacts.Remove(Contact);
                await _context.SaveChangesAsync();
            }
            StatusMessage = $"You just deleted contact of user: {Contact.Name}";
            return RedirectToPage("./Index"); // Chuyển hướng sau khi xóa thành công, có thể thay đổi đường dẫn tùy ý
        }
    }
}
