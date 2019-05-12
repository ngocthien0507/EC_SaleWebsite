namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TmdtDbContext : DbContext
    {
        public TmdtDbContext()
            : base("name=TmdtDbContext")
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<advertisement> advertisements { get; set; }
        public virtual DbSet<BillAd> BillAds { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<ContentTag> ContentTags { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<Footer> Footers { get; set; }
        public virtual DbSet<LocationAd> LocationAds { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<OrderAdvertisement> OrderAdvertisements { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<SaleRate> SaleRates { get; set; }
        public virtual DbSet<ShopOrder> ShopOrders { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<SystemConfig> SystemConfigs { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TotalOrder> TotalOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<About>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<About>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.CMND)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.advertisements)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.Merchant);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.OrderAdvertisements)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.IDmerchant);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Products1)
                .WithOptional(e => e.Account1)
                .HasForeignKey(e => e.CreateBy);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Ratings)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.IDMer);

            modelBuilder.Entity<Account>()
                .HasOptional(e => e.SaleRate)
                .WithRequired(e => e.Account);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.TotalOrders)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.CustomerID);

            modelBuilder.Entity<advertisement>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<advertisement>()
                .HasMany(e => e.OrderAdvertisements)
                .WithOptional(e => e.advertisement)
                .HasForeignKey(e => e.IDAd);

            modelBuilder.Entity<BillAd>()
                .Property(e => e.CMND_ID)
                .IsUnicode(false);

            modelBuilder.Entity<BillAd>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<BillAd>()
                .Property(e => e.TradingCode)
                .IsUnicode(false);

            modelBuilder.Entity<BillAd>()
                .Property(e => e.Money)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Category>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<FeedBack>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<FeedBack>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Footer>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<LocationAd>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<LocationAd>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<LocationAd>()
                .HasMany(e => e.advertisements)
                .WithOptional(e => e.LocationAd)
                .HasForeignKey(e => e.Location);

            modelBuilder.Entity<OrderAdvertisement>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderAdvertisement>()
                .HasMany(e => e.BillAds)
                .WithOptional(e => e.OrderAdvertisement)
                .HasForeignKey(e => e.IdOrderAd);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Product)
                .HasForeignKey(e => e.IDProduct);

            modelBuilder.Entity<ProductCategory>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<ProductCategory>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.ProductCategory)
                .HasForeignKey(e => e.CategoryID);

            modelBuilder.Entity<ShopOrder>()
                .Property(e => e.TotalPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ShopOrder>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.ShopOrder)
                .HasForeignKey(e => e.IDShopOrder);

            modelBuilder.Entity<Slide>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Slide>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<SystemConfig>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<SystemConfig>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<TotalOrder>()
                .Property(e => e.TotalPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TotalOrder>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<TotalOrder>()
                .HasMany(e => e.ShopOrders)
                .WithOptional(e => e.TotalOrder)
                .HasForeignKey(e => e.IDTotalOrder);
        }
    }
}
