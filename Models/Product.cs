using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace do_an_ltweb.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required, StringLength(100)]
        public string? NameProduct { get; set; }

        [Range(0.01, 10000000.00)]
        public decimal Price { get; set; }

        public int Nums { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? Size { get; set; }

        public List<ProductImage>? Images { get; set; }

        [Required, ForeignKey("IdCategoryBrand")]
        public CategoryBrand? CategoryBrand { get; set; }
        public int? IdCategoryBrand { get; set; }

        [ForeignKey("IdCategoryFrameColor")]
        public CategoryFrameColor? CategoryFrameColor { get; set; }
        public int? IdCategoryFrameColor { get; set; }

        [ForeignKey("IdCategoryFrameStyle")]
        public CategoryFrameStyle? CategoryFrameStyle { get; set; }
        public int? IdCategoryFrameStyle { get; set; }

        [ForeignKey("IdCategoryIrisColor")]
        public CategoryIrisColor? CategoryIrisColor { get; set; }
        public int? IdCategoryIrisColor { get; set; }

        [ForeignKey("IdCategoryMaterial")]
        public CategoryMaterial? CategoryMaterial { get; set; }
        public int? IdCategoryMaterial { get; set; }

        [ForeignKey("IdCategoryPrice")]
        public CategoryPrice? CategoryPrice { get; set; }
        public int? IdCategoryPrice { get; set; }

        [ForeignKey("IdCategorySex")]
        public CategorySex? CategorySex { get; set; }
        public int? IdCategorySex { get; set; }

        [ForeignKey("IdCategoryOrigin")]
        public CategoryOrigin? CategoryOrigin { get; set; }
        public int? IdCategoryOrigin { get; set; }

        public int? Hide { get; set; }

    }
}
