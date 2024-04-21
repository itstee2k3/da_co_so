using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class CategoryOrigin
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
 
INSERT INTO[do_an_ltweb].[dbo].CategoryOrigins
([NameCategory], [Hide])
VALUES
    ('United States', 0),
    ('United Kingdom', 0),
    ('Canada', 0),
    ('Australia', 0),
    ('Germany', 0); 
 
*/