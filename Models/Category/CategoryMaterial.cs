using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class CategoryMaterial
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
 
INSERT INTO[do_an_ltweb].[dbo].CategoryMaterials
([NameCategory], [Hide])
VALUES
    ('Metal', 0),

    ('Plastic', 0),

    ('Acetate', 0),

    ('Titanium', 0),

    ('Nylon', 0),

    ('TR90', 0),

    ('Stainless steel', 0); 
 
*/