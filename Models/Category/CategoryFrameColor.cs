using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class CategoryFrameColor
{
    [Key]
    public int IdCategory { get; set; }

    [Required, StringLength(100)]
    public string NameCategory { get; set; }

    //public int? Order { get; set; }

    //public string Link { get; set; }

    public int? Hide { get; set; }

    public List<Product>? Products { get; set; }
}

/*

INSERT INTO[do_an_ltweb].[dbo].[CategoryFrameColors]
([NameCategory],  [Hide])
VALUES

    ('Red',  0),

    ('Yellow',  0),
   
    ('Blue',  0),

    ('Orange',  0),

    ('Green',  0),

    ('Violet',  0),

    ('Yellow-orange',  0),

    ('Red-violet',  0),

    ('Blue-violet',  0),

    ('Yellow-green',  0);
 
*/
