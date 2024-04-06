using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class OrderDetail
{
    [Key]
    public int IdOrderDetail { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("IdOrder")]
    public Order Order { get; set; }
    public int IdOrder { get; set; }

    [ForeignKey("IdProduct")]
    public Product Product { get; set; }
    public int IdProduct { get; set; }
}
