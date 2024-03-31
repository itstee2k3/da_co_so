using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;

namespace do_an_ltweb.Models;

public partial class Feedback
{
    [Key]
    public int IdFeedback { get; set; }

    [Required, StringLength(250)]
    public string Title { get; set; }

    public string Description { get; set; }

    public string? Image { get; set; }

    public DateTime? Datebegin { get; set; }

    public int? Hide { get; set; }

    // Liên kết với một người dùng
    //[Required]
    public int IdUser { get; set; }  // Kiểu dữ liệu không thể null, vì mỗi phản hồi phải thuộc về một người dùng
    public User User { get; set; }
    //public virtual User IdUserNavigation { get; set; } // Navigation property để truy cập thông tin người dùng

    // Liên kết với một sản phẩm đã mua
    [Required]
    public int IdProduct { get; set; } // Kiểu dữ liệu không thể null, vì mỗi phản hồi phải liên kết với một sản phẩm
    public Product Product { get; set; }
    //public virtual Product IdProductNavigation { get; set; } // Navigation property để truy cập thông tin sản phẩm
}
