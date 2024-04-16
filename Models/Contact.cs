using System.ComponentModel.DataAnnotations;

namespace do_an.Models
{
    public class Contact
    {
        [Key]
        public int IdContact { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [StringLength(100)]
        public string Message { get; set; }

        public DateTime Time { get; set; } // Thêm thuộc tính thời gian
    }
}
