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

        public DateTime? Datebegin { get; set; }

        public int? Hide { get; set; }

        // Liên kết với người dùng
        [ForeignKey("IdUser")]
        public User User { get; set; } // Navigation property
        public string IdUser { get; set; }

        public int? Status { get; set; }

        //public List<OrderDetail>? OrderDetails { get; set; }

        // Chi tiết đơn hàng
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
