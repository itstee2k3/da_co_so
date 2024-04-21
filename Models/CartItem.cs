using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;

namespace do_an_ltweb.Models;

public class CartItem
{
    [Key]
    public int IdCartItem { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("IdProduct")]
    public Product Product { get; set; } // Đối tượng Product tương ứng với ProductId
    public int IdProduct { get; set; }

    [ForeignKey("IdUser")]
    public User User { get; set; } // Đối tượng User tương ứng với UserId
    public string IdUser { get; set; } // Tùy chọn, nếu bạn muốn liên kết giỏ hàng với người dùng

}