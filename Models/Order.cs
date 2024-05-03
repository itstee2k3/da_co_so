using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;

namespace do_an_ltweb.Models
{
    public partial class Order 
    {
        [Key]
        public int IdOrder { get; set; }

        public string Address { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public decimal TotalBill { get; set; } // Thêm thuộc tính TotalBill để lưu tổng giá trị đơn hàng

        // Liên kết với người dùng
        [ForeignKey("IdUser")]
        public User User { get; set; } // Navigation property
        public string IdUser { get; set; }

        public int PaymentMethod { get; set; }

        public int StatusUser { get; set; }

        public int StatusAdmin { get; set; }

        // Chi tiết đơn hàng
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
