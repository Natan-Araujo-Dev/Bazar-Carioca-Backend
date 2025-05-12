using BazarCarioca.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //teste
        public DbSet<Shopkeeper>? Shopkeepers { get; set; }
        public DbSet<Store>? Stores { get; set; }
        public DbSet<Service>? Services { get; set; }
        public DbSet<ProductType>? ProductTypes { get; set; }
        public DbSet<Product>? Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Shopkeeper

            // ========== Entidadade ==========
            modelBuilder.Entity<Shopkeeper>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Shopkeeper>().Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            //Lembrando que a string Email tem a propieadade [EmailAddress] em sua própria classe
            modelBuilder.Entity<Shopkeeper>().Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Shopkeeper>().Property(s => s.Password)
                .IsRequired()
                .HasMaxLength(50);

            // ========== Relacionamento ==========
            modelBuilder.Entity<Shopkeeper>()
                .HasMany(sk => sk.Stores)
                .WithOne(s => s.Shopkeeper)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Store

            // ========== Entidade ==========
            modelBuilder.Entity<Store>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Store>().Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Store>().Property(s => s.Description)
                .IsRequired(false)
                .HasMaxLength(80);

            modelBuilder.Entity<Store>().Property(s => s.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(300);

            /* Limitado à 9 caracteres pois:
             * - modelo de número do RJ-RJ é: +55 21 9XXXXXXXX;
             * - sendo o "+55 21 9" comum à todos números;
             * - Obs.: o "9" é posto manualmente pois esse é o modelo que sites grandes usam */
            modelBuilder.Entity<Store>().Property(s => s.CellphoneNumber)
                .IsRequired(false)
                .HasMaxLength(9);

            modelBuilder.Entity<Store>().Property(s => s.Neighborhood)
                .IsRequired(false)
                .HasMaxLength(64);

            modelBuilder.Entity<Store>().Property(s => s.Street)
                .IsRequired(false)
                .HasMaxLength(64);

            modelBuilder.Entity<Store>().Property(s => s.Number)
                .IsRequired(false)
                .HasMaxLength(5);

            modelBuilder.Entity<Store>().Property(s => s.OpeningTime)
                .IsRequired(false)
                .HasMaxLength(4);

            modelBuilder.Entity<Store>().Property(s => s.ClosingTime)
                .IsRequired(false)
                .HasMaxLength(4);

            // ========== Relacionamento ==========
            
            modelBuilder.Entity<Store>()
                .HasMany(s => s.Services)
                .WithOne(serv => serv.Store)
                .HasForeignKey("StoreId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.ProductTypes)
                .WithOne(pt => pt.Store)
                .HasForeignKey("StoreId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Store>()
                .HasOne(s => s.Shopkeeper)
                .WithMany(sk => sk.Stores)
                // shadow FK
                .HasForeignKey("ShopkeeperId")    
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Service

            // ========== Entidade ==========
            modelBuilder.Entity<Service>()
                .HasKey(serv => serv.Id);

            modelBuilder.Entity<Service>().Property(serv => serv.Name)
                .IsRequired()
                .HasMaxLength(80);

            modelBuilder.Entity<Service>().Property(serv => serv.AveragePrice)
                .IsRequired(false)
                .HasPrecision(8, 2);

            // ========== Relacionamento ==========
            modelBuilder.Entity<Service>()
                .HasOne(serv => serv.Store)
                .WithMany(serv => serv.Services)
                // Shadow FK
                .HasForeignKey("StoreId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region ProductType

            // ========== Entidade ==========
            modelBuilder.Entity<ProductType>()
                .HasKey(pt => pt.Id);

            modelBuilder.Entity<ProductType>().Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(80);

            // ========== Relacionamento ==========
            modelBuilder.Entity<ProductType>()
                .HasMany(pt => pt.Products)
                .WithOne(p => p.ProductType)
                .HasForeignKey("ProductTypeId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductType>()
                .HasOne(pt => pt.Store)
                .WithMany(s => s.ProductTypes)
                .HasForeignKey("StoreId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Product

            // ========== Entidade ==========
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>().Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(80);

            modelBuilder.Entity<Product>().Property(p => p.Price)
                .IsRequired()
                .HasPrecision(8, 2);

            modelBuilder.Entity<Product>().Property(p => p.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(300);

            modelBuilder.Entity<Product>().Property(p => p.Stock)
                .IsRequired(false)
                .HasMaxLength(5);

            modelBuilder.Entity<Product>().Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(80);

            // ========== Relacionamento ==========
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany(pt => pt.Products)
                .HasForeignKey("ProductTypeId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
