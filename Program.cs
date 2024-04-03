using do_an_ltweb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Album.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryBrand, EFCategoryBrand>();
builder.Services.AddScoped<ICategoryFrameColor, EFCategoryFrameColor>();
builder.Services.AddScoped<ICategoryFrameStyle, EFCategoryFrameStyle>();
builder.Services.AddScoped<ICategoryIrisColor, EFCategoryIrisColor>();
builder.Services.AddScoped<ICategoryMaterial, EFCategoryMaterial>();
builder.Services.AddScoped<ICategoryOrigin, EFCategoryOrigin>();
builder.Services.AddScoped<ICategoryPrice, EFCategoryPrice>();
builder.Services.AddScoped<ICategorySex, EFCategorySex>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DockerConnection")));

builder.Services.AddIdentity<User, IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddRoles<IdentityRole>()
    //.AddUserManager<IdentityUser>()
    .AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/cannotaccess.html";
});
builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Khóa 2 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lần thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;          // Xác thực account
});

builder.Services.AddAuthorization(options => {

    options.AddPolicy("AllowEditRole", policyBuilder => {
        // Dieu kien cua Policy
        policyBuilder.RequireAuthenticatedUser();
        //policyBuilder.RequireRole("Admin");
        // policyBuilder.RequireRole("Editor");

        // policyBuilder.RequireClaim("manage.role", "add", "update");
        policyBuilder.RequireClaim("canedit","role");


        // Claims-based authorization
        // policyBuilder.RequireClaim("Ten Claim", "giatri1", "giatri2");
        // policyBuilder.RequireClaim("Ten Claim", new string[] {
        //     "giatri1",
        //     "giatri2"
        // });

        // IdentityRoleClaim<string> claim1; ->DbContext
        // IdentityUserClaim<string> claim2; ->DbContext
        // Claim claim3; -> tu dich vu cua Identity

    });
});

builder.Services.AddOptions();                                        // Kích hoạt Options
var mailsettings = builder.Configuration.GetSection("MailSettings");  // đọc config
builder.Services.Configure<MailSettings>(mailsettings);               // đăng ký để Inject

builder.Services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

//app.UseEndpoints(endpoints =>
//{
//endpoints.MapControllerRoute(
//        name: "Admin",
//        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//});

app.MapControllerRoute(
name: "default",
pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
