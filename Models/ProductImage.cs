using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public string? Url { get; set; }

        [ForeignKey("IdProduct")]
        public Product? Product { get; set; }
        public int IdProduct { get; set; }
    }
}
