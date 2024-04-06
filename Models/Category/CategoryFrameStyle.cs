using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;

namespace do_an_ltweb.Models;

public partial class CategoryFrameStyle
{
    [Key]
    public int IdCategory { get; set; }

    [Required, StringLength(100)]
    public string NameCategory { get; set; }

    //public int? Order { get; set; }

    public string? ImageUrl { get; set; }

    public int? Hide { get; set; }

    public List<Product>? Products { get; set; }
}

/*

INSERT INTO [do_an_ltweb].[dbo].[CategoryFrameStyles] ([NameCategory], [ImageUrl], [Hide])
VALUES
    -- mắt mèo
    ('Cats eyes', 'images/frame_style_1.webp', 0),
    -- bướm
    ('Butterfly', 'images/frame_style_2.webp', 0),
    -- oval
    ('Oval', 'images/frame_style_3.webp', 0),
    -- vuông
    ('Square', 'images/frame_style_4.webp', 0),
    -- phi công
    ('Pilot', 'images/frame_style_5.webp', 0),
    -- chữ nhật
    ('Rectangle', 'images/frame_style_6.webp', 0),
    -- đa giác
    ('Polygon', 'images/frame_style_7.webp', 0),
    -- thể thao
    ('Sports', 'images/frame_style_8.webp', 0),
    -- tròn
    ('Round', 'images/frame_style_9.webp', 0),

    ('Pillow', 'images/frame_style_10.webp', 0),
    ('Browline', 'images/frame_style_11.webp', 0),
    ('Phantos', 'images/frame_style_12.webp', 0),
    ('Mask', 'images/frame_style_13.webp', 0),
    -- phi hình học
    ('Non-geometric', Null, 0);
 
*/