using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Contexts;

public partial class VFoodyContext : DbContext
{
    public VFoodyContext()
    {
    }

    public VFoodyContext(DbContextOptions<VFoodyContext> options)
        : base(options)
    {
    }

    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in this.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
        {
            var now = DateTime.Now;
            entry.Property("UpdatedDate").CurrentValue = now;
            if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedDate").IsModified = false;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedDate").CurrentValue = now;
            }
        }

        var numberChange = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        this.ChangeTracker.Clear();
        return numberChange;
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CommissionConfig> CommissionConfigs { get; set; }

    public virtual DbSet<Contribution> Contributions { get; set; }

    public virtual DbSet<ContributionType> ContributionTypes { get; set; }

    public virtual DbSet<Distance> Distances { get; set; }

    public virtual DbSet<FavouriteShop> FavouriteShops { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Order> Orders { get; set; }
    
    public virtual DbSet<OrderHistory> OrderHistories { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderDetailOption> OrderDetailOptions { get; set; }

    public virtual DbSet<PersonPromotion> PersonPromotions { get; set; }

    public virtual DbSet<PlatformPromotion> PlatformPromotions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<ShopPromotion> ShopPromotions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }
    
    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }
    
    public virtual DbSet<VerificationCode> VerificationCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(Environment.GetEnvironmentVariable("DATABASE_URL"), ServerVersion.Parse("8.0.33-mysql"));

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config.GetConnectionString("DBDefault");

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Building).WithMany(p => p.Accounts).HasConstraintName("account_building_FK");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("account_role_FK");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<CommissionConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Contribution>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Account).WithMany(p => p.Contributions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contribution_account_FK");

            entity.HasOne(d => d.ContributionType).WithMany(p => p.Contributions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contribution_contribution_type_FK");
        });

        modelBuilder.Entity<ContributionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Distance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.BuildingIdFromNavigation).WithMany(p => p.DistanceBuildingIdFromNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("distance_from_building_FK");

            entity.HasOne(d => d.BuildingIdToNavigation).WithMany(p => p.DistanceBuildingIdToNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("distance_to_building_FK");
        });

        modelBuilder.Entity<FavouriteShop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Account).WithMany(p => p.FavouriteShops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favourite_shop_account_FK");

            entity.HasOne(d => d.Shop).WithMany(p => p.FavouriteShops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favourite_shop_shop_FK");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Account).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("feedback_account_FK");

            entity.HasOne(d => d.Order).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("feedback_order_FK");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Readed).HasDefaultValueSql("b'0'");

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notification_account_FK");

            entity.HasOne(d => d.Role).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notification_role_FK");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.IsPricing).HasDefaultValueSql("b'0'");

            entity.HasOne(d => d.Question).WithMany(p => p.Options)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("option_question_FK");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_account_FK");

            entity.HasOne(d => d.PersonalPromotion).WithMany(p => p.Orders).HasConstraintName("order_person_promotion_FK");

            entity.HasOne(d => d.PlatformPromotion).WithMany(p => p.Orders).HasConstraintName("order_platform_promotion_FK");

            entity.HasOne(d => d.Shop).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_shop_FK");

            entity.HasOne(d => d.ShopPromotion).WithMany(p => p.Orders).HasConstraintName("order_shop_promotion_FK");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_transaction_FK");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_detail_order_FK");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_detail_product_FK");
        });

        modelBuilder.Entity<OrderDetailOption>(entity =>
        {
            entity.HasKey(e => new { e.OrderDetailId, e.OptionId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasOne(d => d.Option).WithMany(p => p.OrderDetailOptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_detail_option_option_fk");

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.OrderDetailOptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_detail_option_order_detail_fk");
        });

        modelBuilder.Entity<OrderHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.IsRefund).HasDefaultValueSql("b'0'");
        });

        modelBuilder.Entity<PersonPromotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Account).WithMany(p => p.PersonPromotions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("person_promotion_account_FK");
        });

        modelBuilder.Entity<PlatformPromotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_shop_FK");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.ProductId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasOne(d => d.Category).WithMany(p => p.ProductCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_category_category_FK");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_category_product_FK");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Product).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("question_product_FK");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Active).HasDefaultValueSql("b'0'");

            entity.HasOne(d => d.Account).WithMany(p => p.Shops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shop_account_FK");

            entity.HasOne(d => d.Building).WithMany(p => p.Shops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shop_building_FK");
        });

        modelBuilder.Entity<ShopPromotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Shop).WithMany(p => p.ShopPromotions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shop_promotion_shop_FK");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });
        
        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<VerificationCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Account).WithMany(p => p.VerificationCodes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("verification_code_account_FK");
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
