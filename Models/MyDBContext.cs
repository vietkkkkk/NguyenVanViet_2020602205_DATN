using Microsoft.EntityFrameworkCore;

using webbanhang.Models;

namespace webbanhang.Models
{
    public class MyDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            //string strConnectionString = "Server=MSI\\SQLEXPRESS01;Database=dotnet23_project;Integrated Security=True;TrustServerCertificate=True;";
            string strConnectionString = config.GetConnectionString("MyConnectionString");
            optionsBuilder.UseSqlServer(strConnectionString);
        }
        public DbSet<ItemAdmin> Admins { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ItemCategory> Categories { get; set; }
        public DbSet<ItemAdv> Adv { get; set; }
        public DbSet<ItemCustomer> Customers { get; set; }
        public DbSet<ItemNews> News { get; set; }
        public DbSet<ItemOrder> Orders { get; set; }
        public DbSet<ItemOrderDetail> OrdersDetail { get; set; }
        public DbSet<ItemProduct> Products { get; set; }
        public DbSet<ItemRating> Rating { get; set; }
        public DbSet<ItemSlide> Slides { get; set; }
        public DbSet<ItemTag> Tags { get; set; }
        public DbSet<ItemTagProduct> TagsProducts { get; set; }
        public DbSet<ItemCategoryProduct> CategoriesProducts { get; set; }



    }
}
