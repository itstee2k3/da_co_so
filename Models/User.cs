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

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}

/*
 
DECLARE @counter INT = 1;
WHILE @counter <= 100
BEGIN
    INSERT INTO [dbo].[Users] (
        [Id],
        [Address],
        [BirthDate],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumber],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnd],
        [LockoutEnabled],
        [AccessFailedCount]
    )
    VALUES (
        NEWID(), -- Tạo một GUID ngẫu nhiên cho Id
        'Address' + CAST(@counter AS NVARCHAR(3)), -- Địa chỉ
        DATEADD(YEAR, -20, GETDATE()), -- Ngày sinh
        'User' + CAST(@counter AS NVARCHAR(3)), -- Tên người dùng
        'USER' + CAST(@counter AS NVARCHAR(3)), -- Tên người dùng chuẩn hóa
        'user' + CAST(@counter AS NVARCHAR(3)) + '@example.com', -- Email
        'USER' + CAST(@counter AS NVARCHAR(3)) + '@EXAMPLE.COM', -- Email chuẩn hóa
        1, -- Đã xác nhận Email
        'passwordhash', -- Hash mật khẩu
        'securitystamp', -- Stamp bảo mật
        NEWID(), -- Concurrency stamp
        '1234567890', -- Số điện thoại
        1, -- Đã xác nhận số điện thoại
        0, -- Không kích hoạt 2 yếu tố
        NULL, -- Khóa tài khoản hết hạn
        1, -- Khóa tài khoản được bật
        0 -- Số lần đăng nhập không thành công
    );

    SET @counter = @counter + 1;
END;
 
*/