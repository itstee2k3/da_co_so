using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class CategoryIrisColor
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

INSERT INTO[do_an_ltweb].[dbo].CategoryIrisColors
([NameCategory], [Hide])
VALUES

    ('Black', 0),

    ('Brown', 0),

    ('Navy blue', 0),

    ('Grey', 0),

    ('Havana', 0),

    ('Rose gold', 0),

    ('Gray mix', 0),

    ('Gold', 0),

    ('Gunmetal', 0),

    ('Silver', 0); 
 
*/