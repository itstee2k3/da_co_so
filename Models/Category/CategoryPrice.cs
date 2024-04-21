using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class CategoryPrice
{
    [Key]
    public int IdCategory { get; set; }

    [Required, StringLength(100)]
    public string NameCategory { get; set; }

    //public int? Order { get; set; }

    //public string Link { get; set; }

    public int? Hide { get; set; }

    public int? PriceMin { get; set; }

    public int? PriceMax { get; set; }
}

/*
 
INSERT INTO[do_an_ltweb].[dbo].CategoryPrices
([NameCategory], [Hide], [PriceMin], [PriceMax])
VALUES
    ('Under 500,000₫', 0, NULL, 500000),
    ('500,000₫ - 1,500,000₫', 0, 500000, 1500000),
    ('1,500,000₫ - 3,000,000₫', 0, 1500000, 3000000),
    ('3,000,000₫ - 5,000,000₫', 0, 3000000, 5000000),
    ('Above 5,000,000₫', 0, 5000000, NULL); 
 
*/