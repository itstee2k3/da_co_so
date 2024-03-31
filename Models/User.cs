using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace do_an_ltweb.Models;

public partial class User : IdentityUser
{
    //[Key]
    //public int IdUsers { get; set; }

    //public string? Username { get; set; }

    //public string? Password { get; set; }

    //public string? Name { get; set; }

    [Column(TypeName = "nvarchar")]
    [StringLength(400)]
    public string? Address { get; set; }

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    //public string? Phone { get; set; }

    //public int? Permission { get; set; }

    //public int? Hide { get; set; }

    //public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    //public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
