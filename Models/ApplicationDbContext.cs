using System.Reflection.Metadata;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace do_an_ltweb.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<CategoryBrand> CategoryBrands { get; set; }
        public DbSet<CategoryFrameColor> CategoryFrameColors { get; set; }
        public DbSet<CategoryFrameStyle> CategoryFrameStyles { get; set; }
        public DbSet<CategoryIrisColor> CategoryIrisColors { get; set; }
        public DbSet<CategoryMaterial> CategoryMaterials { get; set; }
        public DbSet<CategoryPrice> CategoryPrices { get; set; }
        public DbSet<CategorySex> CategorySexes { get; set; }
        public DbSet<CategoryOrigin> CategoryOrigins { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        //public DbSet<Slider> Sliders { get; set; }
        //public DbSet<About> Abouts { get; set; }
        //public DbSet<Contact> Contacts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Product>()
            //    .Property(p => p.Price)
            //    .HasColumnType("decimal(18,2)"); // Chỉ định kiểu và scale cho thuộc tính Price

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            { 
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
    }
}