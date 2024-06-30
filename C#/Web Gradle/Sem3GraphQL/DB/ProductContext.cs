using Microsoft.EntityFrameworkCore;
using GBLesson3GraphQL.Models;

namespace GBLesson3GraphQL.DB
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<ProductGroup> ProductGroup { get; set; }

        private string _connectionString = "Host=localhost;Port=5432;Username=aaa;Password=1234;Database=Products";   //

        public ProductContext() //
        {
        }

        public ProductContext(string connectionString)  //
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql(_connectionString);    //
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id).HasName("product_pkey");
                entity.ToTable("products");
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.Name)
                      .HasMaxLength(255)
                      .HasColumnName("name");
                entity.Property(p => p.Description)
                      .HasMaxLength(1024)
                      .HasColumnName("description");
                entity.Property(d => d.Price).HasColumnType("float").HasColumnName("price");

                entity.HasOne(p => p.ProductGroup).WithMany(pg => pg.Products).HasForeignKey(p => p.ProductGroupId);
            });

            modelBuilder.Entity<ProductGroup>(entity =>
            {
                entity.HasKey(pg => pg.Id).HasName("productgroup_pkey");
                entity.ToTable("productgroups");
                entity.Property(pg => pg.Id).HasColumnName("id");
                entity.Property(pg => pg.Name).HasColumnName("name");
                entity.Property(pg => pg.Description).HasColumnName("description");
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(s => s.Id).HasName("storage_pkey");
                entity.ToTable("storages");
                entity.Property(s => s.Id).HasColumnName("id");
            });

            modelBuilder.Entity<ProductStorage>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.StorageId }).HasName("product_storage_pkey");
                entity.HasOne(ps => ps.Storage).WithMany(s => s.Products).HasForeignKey(ps => ps.StorageId);
                entity.HasOne(ps => ps.Product).WithMany(s => s.Storages).HasForeignKey(ps => ps.ProductId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}

// в терминале для ChatBD  -  dotnet tool install --global dotnet-ef --version 8.0.4
// далее  -  dotnet ef migrations add InitialCreate. Получили два файла в каталоге Migrations.
// для изменений в БД  -  dotnet ef database update
