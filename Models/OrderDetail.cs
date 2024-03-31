using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class OrderDetail
{
    [Key]
    public int IdOrderDetail { get; set; }

    public int? SoldNum { get; set; }

    public int? Hide { get; set; }

    public int IdOrder { get; set; }
    public Order Order { get; set; }
    //public virtual Order IdOrderNavigation { get; set; }

    public int IdProduct { get; set; }
    public virtual Product IdProductNavigation { get; set; }
}
