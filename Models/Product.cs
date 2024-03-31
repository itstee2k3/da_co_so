using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace do_an_ltweb.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required, StringLength(100)]
        public string? NameProduct { get; set; }

        [Range(1, 10000000)]
        public double Price { get; set; }

        public int? Nums { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public List<ProductImage>? Images { get; set; }

        public int IdCategory { get; set; }
        //public Category IdCategoryNavigation { get; set; }
        public Category Category { get; set; }

        public int? Hide { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    }
}
