using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public string? Url { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
