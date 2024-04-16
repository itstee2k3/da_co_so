using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class CategoryBrand
{
    [Key]
    public int IdCategory { get; set; }

    [Required, StringLength(100)]
    public string NameCategory { get; set; }

    public int? Hide { get; set; }

    public List<Product>? Products { get; set; }
}

/*

INSERT INTO [do_an_ltweb].[dbo].[CategoryBrands] ([NameCategory], [Hide])
VALUES
    ('EMPORIO ARMANI', 0),
    ('PRADA', 0),
    ('BOLON', 0),
    ('TOMMY HILFIGER', 0),
    ('VERSACE', 0),
    ('BURBERRY', 0),
    ('RAYBAN', 0),
    ('PUMA', 0),
    ('FLYER', 0),
    ('ARMANI EXCHANGE', 0);


EMPORIO ARMANI
PRADA
BOLON
TOMMY HILFIGER
VERSACE
BURBERRY
RAYBAN
PUMA
FLYER
ARMANI EXCHANGE

MOLSION
SWAROVSKI
AMO
TIFFANY & CO.
DOLCE & GABBANA
OAKLEY
BVLGARI
GUCCI
FOSSIL
MONTBLANC
ANCCI
HANGTEN
REVLON
YVES SAINT LAURENT
REEBOK
GIORGIO FERRI
MINIMA
VALENTINO RUDY
VOGUE
TOMMY JEANS
KALLA
MICHAEL KORS
COACH
GUY LAROCHE
NIKON
BOTTEGA VENETA
ILOOK
FOSTER GRANT
ALAIN DELON
*/