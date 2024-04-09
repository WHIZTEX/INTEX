using INTEX.Models.DatabaseModels;
using INTEX.Models.DatabaseModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace INTEX.Models.Infrastructure;


public partial class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext()
    {
    }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
    
    public virtual DbSet<Address> Addresses { get; set; }
    
    public virtual DbSet<Customer> Customers { get; set; }
    
    public virtual DbSet<Transaction> Transactions { get; set; }
    
    public virtual DbSet<Order> Orders { get; set; }
    
    public virtual DbSet<Product> Products { get; set; }
    
    public virtual DbSet<LineItem> LineItems { get; set; }
    
    public virtual DbSet<Rating> Ratings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=INTEX");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasIndex(e => new { e.AddressLine1, e.AddressLine2, e.City, e.State, e.Code, e.Country }).IsUnique();
            entity.HasIndex(e => e.AddressLine1, "AddressLine1Index");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.AddressLine1).HasMaxLength(64).IsRequired();
            entity.Property(e => e.AddressLine2).HasMaxLength(64);
            entity.Property(e => e.City).HasMaxLength(64);
            entity.Property(e => e.State).HasMaxLength(64);
            entity.Property(e => e.Code).HasMaxLength(64);
            entity.Property(e => e.Country).HasMaxLength(64).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.AspNetUserId).IsUnique();
            entity.HasIndex(e => e.FirstName, "FirstNameIndex");
            entity.HasIndex(e => e.LastName, "LastNameIndex");
            entity.HasIndex(e => e.BirthDate, "BirthDateIndex");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FirstName).HasMaxLength(64).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Gender).HasMaxLength(1).IsRequired();
            entity.Property(e => e.BirthDate).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            entity.HasOne<AspNetUser>(e => e.AspNetUser)
                .WithOne()
                .HasForeignKey<Customer>(e => e.AspNetUserId);
            entity.HasOne<Address>(e => e.HomeAddress)
                .WithOne()
                .HasForeignKey<Customer>(e => e.AddressId);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasIndex(e => new { e.AddressId, e.DateTime, e.Amount, e.CardType, e.EntryMode, e.Bank }).IsUnique();
            entity.HasIndex(e => e.DateTime, "TransactionDateIndex");
            entity.HasIndex(e => e.Bank, "BankIndex");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CardType).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Bank).HasMaxLength(64).IsRequired();
            entity.Property(e => e.EntryMode).HasMaxLength(8).IsRequired();
            entity.Property(e => e.DateTime).IsRequired();
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            entity.HasOne<Address>(e => e.BillingAddress)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.AddressId)
                .IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => new { e.CustomerId, e.TransactionId, e.AddressId, e.DateTime, e.Type }).IsUnique();
            entity.HasIndex(e => e.DateTime, "OrderDateIndex");
            entity.HasIndex(e => e.Type, "TypeIndex");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Type).HasMaxLength(16).IsRequired();
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.TransactionId).IsRequired();
            entity.Property(e => e.AddressId).IsRequired();
            entity.Property(e => e.DateTime).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            entity.HasOne<Customer>(e => e.Customer)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired();
            entity.HasOne<Transaction>(e => e.Transaction)
                .WithOne()
                .HasForeignKey<Order>(e => e.TransactionId)
                .IsRequired();
            entity.HasOne<Address>(e => e.ShippingAddress)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.AddressId)
                .IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.Name, "ProductNameIndex").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(128).IsRequired();
            entity.Property(e => e.ImgLink).HasMaxLength(256).IsRequired();
            entity.Property(e => e.PrimaryColor).HasMaxLength(16).IsRequired();
            entity.Property(e => e.SecondaryColor).HasMaxLength(16).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(4096).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(64).IsRequired();
            entity.Property(e => e.ReleaseYear).IsRequired();
            entity.Property(e => e.Pieces).IsRequired();
            entity.Property(e => e.Price).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasIndex(e => new { e.OrderId, e.ProductId }).IsUnique();
            entity.HasIndex(e => e.OrderId, "LineOrderIndex");
            entity.HasIndex(e => e.ProductId, "LineProductIndex");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            entity.HasOne<Order>(e => e.Order)
                .WithMany(e => e.LineItems)
                .HasForeignKey(e => e.OrderId);
            entity.HasOne<Product>(e => e.Product)
                .WithMany(e => e.LineItems)
                .HasForeignKey(e => e.ProductId)
                .IsRequired();
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasIndex(e => new { e.CustomerId, e.ProductId }).IsUnique();
            entity.HasIndex(e => e.CustomerId, "CustomerRatingIndex");
            entity.HasIndex(e => e.ProductId, "ProductRatingIndex");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Score).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            entity.HasOne<Customer>(e => e.Customer)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired();
            entity.HasOne<Product>(e => e.Product)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.ProductId)
                .IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}