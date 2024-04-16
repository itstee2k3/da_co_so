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

        public decimal Price { get; set; }

        public int Nums { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? Size { get; set; }

        public List<ProductImage>? Images { get; set; }

        [ForeignKey("IdCategoryBrand")]
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

/*

INSERT INTO [do_an_ltweb].[dbo].[Products] ([NameProduct], [Price], [Nums], [Description], [ImageUrl], [Size], [IdCategoryBrand], [IdCategoryFrameColor],
[IdCategoryFrameStyle], [IdCategoryIrisColor], [IdCategoryMaterial], [IdCategoryPrice], [IdCategorySex], [IdCategoryOrigin], [Hide])
VALUES
    ('Kính Mát Nữ PRADA 0PR07YSF 14213055', 10180000, 100, Null, '/images/glass_1.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Unisex PRADA 0PR06YSF 09Q5S054', 10480000, 200, Null, '/images/glass_2.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Nữ PRADA 0PR15WSF 09Q5S055', 9480000, 300, Null, '/images/glass_3.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Nam PRADA 0PR09ZSF 19D5S055', 12880000, 400, Null, '/images/glass_4.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Nữ PRADA 0PR17ZSF 1AB09S55', 7980000, 500, Null, '/images/glass_5.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Nam PRADA 0PR66ZS AAV07T58', 11580000, 600, Null, '/images/glass_6.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Nam PRADA 0PR66ZS YDC03R58', 13380000, 700, Null, '/images/glass_7.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Nam PRADA 0PR60WS 1BO5G158', 8082000, 800, Null, '/images/glass_8.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Unisex PRADA 0PR13WSF 1AB5S057', 5664000, 900, Null, '/images/glass_9.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null),
    ('Kính Mát Nữ PRADA 0PR09WS 1AB5Z154', 6304000, 990, Null, '/images/glass_10.webp', Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);

*/
