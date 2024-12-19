using do_an_ltweb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace do_an.Models
{
    public class Favourite
        {
            [Key]
            public int FavouriteId { get; set; } // Khóa chính


            [ForeignKey("IdUser")]
            public User User { get; set; }
            public String IdUser { get; set; }

            [ForeignKey("IdProduct")]
            public Product Product { get; set; }
            public int IdProduct { get; set; }

        public Favourite() { }
    }
}
