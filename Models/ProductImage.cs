using System.Collections.Generic;
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
//INSERT INTO ProductImages (Url, IdProduct)
//VALUES
//('/images/360/LapTop/Images/Macbook-360-org-1.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-2.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-3.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-4.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-5.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-6.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-7.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-8.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-9.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-10.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-11.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-12.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-13.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-14.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-15.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-16.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-17.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-18.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-19.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-20.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-21.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-22.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-23.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-24.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-25.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-26.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-27.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-28.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-29.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-30.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-31.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-32.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-33.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-34.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-35.jpg', 36),
//('/images/360/LapTop/Images/Macbook-360-org-36.jpg', 36);
