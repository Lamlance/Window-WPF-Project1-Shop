using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WPF_Project1_Shop.EFModel;

public partial class RailwayContext : DbContext
{
  public RailwayContext()
  {
  }

  public RailwayContext(DbContextOptions<RailwayContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Category> Categories { get; set; }

  public virtual DbSet<Customer> Customers { get; set; }

  public virtual DbSet<Order> Orders { get; set; }

  public virtual DbSet<OrderItem> OrderItems { get; set; }

  public virtual DbSet<Product> Products { get; set; }

  #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseNpgsql("Host=containers-us-west-181.railway.app;Port=5457;Database=railway;Username=postgres;Password=8hW9GLBvcosfIKxTNVB3");

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresExtension("timescaledb");

    modelBuilder.Entity<Category>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("categories_pkey");

      entity.ToTable("categories");

      entity.HasIndex(e => e.CategoryName, "categories_category_name_key").IsUnique();

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.CategoryName)
              .HasMaxLength(50)
              .HasColumnName("category_name");
    });

    modelBuilder.Entity<Customer>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("customers_pkey");

      entity.ToTable("customers");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Address).HasColumnName("address");
      entity.Property(e => e.Email)
              .HasMaxLength(50)
              .HasColumnName("email");
      entity.Property(e => e.FirstName)
              .HasMaxLength(10)
              .HasColumnName("first_name");
      entity.Property(e => e.LastName)
              .HasMaxLength(10)
              .HasColumnName("last_name");
      entity.Property(e => e.MiddleName)
              .HasMaxLength(10)
              .HasColumnName("middle_name");
      entity.Property(e => e.Phone)
              .HasMaxLength(10)
              .HasColumnName("phone");
    });

    modelBuilder.Entity<Order>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("orders_pkey");

      entity.ToTable("orders");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.CreatedAt).HasColumnName("created_at");
      entity.Property(e => e.CustomerId).HasColumnName("customer_id");
      entity.Property(e => e.ShipAddress).HasColumnName("ship_address");
      entity.Property(e => e.Status)
              .HasMaxLength(10)
              .HasColumnName("status");
      entity.Property(e => e.Subtotal).HasColumnName("subtotal");
      entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

      entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
              .HasForeignKey(d => d.CustomerId)
              .HasConstraintName("fk_order_customer");
    });

    modelBuilder.Entity<OrderItem>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("order_items_pkey");

      entity.ToTable("order_items");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.CreatedAt).HasColumnName("created_at");
      entity.Property(e => e.OrderId).HasColumnName("order_id");
      entity.Property(e => e.Price).HasColumnName("price");
      entity.Property(e => e.ProductId).HasColumnName("product_id");
      entity.Property(e => e.Quantity).HasColumnName("quantity");
      entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

      entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
              .HasForeignKey(d => d.OrderId)
              .HasConstraintName("fk_oi_order");

      entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
              .HasForeignKey(d => d.ProductId)
              .HasConstraintName("fk_oi_product");
    });

    modelBuilder.Entity<Product>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("products_pkey");

      entity.ToTable("products");

      entity.HasIndex(e => e.ProductName, "products_product_name_key").IsUnique();

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.CreatedAt).HasColumnName("created_at");
      entity.Property(e => e.Descriptions).HasColumnName("descriptions");
      entity.Property(e => e.ImagePath).HasColumnName("image_path");
      entity.Property(e => e.Numbers).HasColumnName("numbers");
      entity.Property(e => e.Price).HasColumnName("price");
      entity.Property(e => e.ProductName)
              .HasMaxLength(50)
              .HasColumnName("product_name");
      entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

      entity.HasMany(d => d.Categories).WithMany(p => p.Products)
              .UsingEntity<Dictionary<string, object>>(
                  "ProductCategory",
                  r => r.HasOne<Category>().WithMany()
                      .HasForeignKey("CategoryId")
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("fk_pc_category"),
                  l => l.HasOne<Product>().WithMany()
                      .HasForeignKey("ProductId")
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("fk_pc_product"),
                  j =>
                  {
                  j.HasKey("ProductId", "CategoryId").HasName("product_category_pkey");
                  j.ToTable("product_category");
                  j.IndexerProperty<long>("ProductId").HasColumnName("product_id");
                  j.IndexerProperty<long>("CategoryId").HasColumnName("category_id");
                });
    });
    modelBuilder.HasSequence("chunk_constraint_name", "_timescaledb_catalog");

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
