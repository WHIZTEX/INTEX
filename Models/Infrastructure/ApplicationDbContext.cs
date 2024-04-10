using INTEX.Models.DatabaseModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace INTEX.Models.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<Customer>
{
    public ApplicationDbContext()
    {
    }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    // DbSets for all explicit table definitions (that have defined classes)
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<LineItem> LineItems { get; set; }
    public virtual DbSet<Rating> Ratings { get; set; }

    /// <summary>
    /// This method is called after instantiation for determining server types
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Name=INTEX", options =>
        {
            options.EnableRetryOnFailure();
        });
    }

    /// <summary>
    /// This method is called after instantiation when creating models to specify
    /// the behavior of the entities (tables).
    /// </summary>
    /// <param name="modelBuilder">The ModelBuilder passed by .NET wizardry</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Calling parent class (super) to configure Identity tables.
        base.OnModelCreating(modelBuilder);
        ConfigureKeys(modelBuilder);
        ConfigureIndexes(modelBuilder);
        ConfigureProperties(modelBuilder);
        ConfigureRelationships(modelBuilder);
    }

    /// <summary>
    /// This method is called to add all of the composite and primary key constraints to the entities
    /// </summary>
    /// <param name="modelBuilder">The forwarded modelBuilder from OnModelCreating</param>
    private void ConfigureKeys(ModelBuilder modelBuilder)
    {
        // Address has a composite key on all fields
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Transaction has a composite key on all fields except Amount
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasAlternateKey(e => new { e.AddressId, e.DateTime, e.CardType, e.EntryMode, e.Bank });
        });

        // Order has a composite key on all Ids, timestamp, and type
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasAlternateKey(e => new { e.CustomerId, e.TransactionId, e.AddressId, e.DateTime, e.Type });
        });

        // Product has a primary key on name
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Name);
        });

        // LineItem has a composite key on OrderId and ProductId
        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasAlternateKey(e => new { e.OrderId, e.ProductId });
        });

        // Rating has a composite key on CustomerId and ProductId
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasAlternateKey(e => new { e.CustomerId, e.ProductId });
        });
        
    }

    /// <summary>
    /// This method is called to add all of the indexes for the entities.
    /// Indexes are determined based on which fields will benefit from fast access.
    /// </summary>
    /// <param name="modelBuilder">The forwarded modelBuilder from OnModelCreating</param>
    private void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasIndex(e => new { e.AddressLine1, e.AddressLine2, e.City, e.State, e.Code, e.Country})
                .IsUnique();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.FirstName);
            entity.HasIndex(e => e.LastName);
            entity.HasIndex(e => e.BirthDate);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasIndex(e => e.DateTime);
            entity.HasIndex(e => e.Bank);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.DateTime);
            entity.HasIndex(e => e.Type);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Name);
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasIndex(e => e.OrderId);
            entity.HasIndex(e => e.ProductId);
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.ProductId);
        });
        
    }

    /// <summary>
    /// This method is called to add all of the property constraints for the entities.
    /// Property decisions are described in comments
    /// </summary>
    /// <param name="modelBuilder">The forwarded modelBuilder from OnModelCreating</param>
    private void ConfigureProperties(ModelBuilder modelBuilder)
    {
        // All Ids are auto-incremented (ValueGeneratedOnAdd)
        
        // All string fields have a max length that is a power of 2
        // - Proper names (Streets, Cities, Names, Banks, etc.) => 64 characters
        // - Qualifiers (EntryMode, Type, Colors, etc.) => 16 characters
        // - Gender => 32 characters
        // - Product Qualifiers (Name, ImgLink) => 256 characters
        // - Blob text (Descriptions) => 4096 characters
        
        // All properties (Excluding Address information that is an added feature) are required
        modelBuilder.Entity<Address>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.AddressLine1).HasMaxLength(64);
            entity.Property(e => e.AddressLine2).HasMaxLength(64);
            entity.Property(e => e.City).HasMaxLength(64);
            entity.Property(e => e.State).HasMaxLength(64);
            entity.Property(e => e.Code).HasMaxLength(64);
            entity.Property(e => e.Country).HasMaxLength(64).IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(64).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Gender).HasMaxLength(32).IsRequired();
            entity.Property(e => e.BirthDate).IsRequired();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CardType).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Bank).HasMaxLength(64).IsRequired();
            entity.Property(e => e.EntryMode).HasMaxLength(16).IsRequired();
            entity.Property(e => e.DateTime).IsRequired();
            // Decimal rounding properties specified for precision
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)").IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Type).HasMaxLength(16).IsRequired();
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.TransactionId).IsRequired();
            entity.Property(e => e.AddressId).IsRequired();
            entity.Property(e => e.DateTime).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(256).IsRequired();
            entity.Property(e => e.ImgLink).HasMaxLength(256).IsRequired();
            entity.Property(e => e.PrimaryColor).HasMaxLength(16).IsRequired();
            entity.Property(e => e.SecondaryColor).HasMaxLength(16).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(4096).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(64).IsRequired();
            entity.Property(e => e.ReleaseYear).IsRequired();
            entity.Property(e => e.Pieces).IsRequired();
            // Decimal rounding properties specified for precision
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)").IsRequired();
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Quantity).IsRequired();
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Score).IsRequired();
        });
        
    }

    /// <summary>
    /// This method is called to add all of the 
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            // Customers have a one to one required relationship with Address
            // Addresses have a one to many optional reverse relationship with Customers
            entity.HasOne<Address>(e => e.HomeAddress)
                .WithOne()
                .HasForeignKey<Customer>(e => e.AddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            // Transactions have a one to one required relationship with Addresses
            // Addresses have a one to mny optional reverse relationship with Transactions
            entity.HasOne<Address>(e => e.BillingAddress)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            // Orders have a one to one required relationship with Customers
            // Customers have a one to many optional reverse relationship with Orders
            entity.HasOne<Customer>(e => e.Customer)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            // Orders have a one to one required relationship with Transactions
            // Transactions have a one to one required reverse relationship with Orders
            entity.HasOne<Transaction>(e => e.Transaction)
                .WithOne()
                .HasForeignKey<Order>(e => e.TransactionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            // Orders have a one to one required relationship with Addresses
            // Addresses have a one to many optional reverse relationship with Orders
            entity.HasOne<Address>(e => e.ShippingAddress)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            // LineItems have a one to one required relationship with Orders
            // Orders have a one to many required reverse relationship with LineItems
            entity.HasOne<Order>(e => e.Order)
                .WithMany(e => e.LineItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            // LineItems have a one to one required relationship with Products
            // Products have a one to many required reverse relationship with LineItems
            entity.HasOne<Product>(e => e.Product)
                .WithMany(e => e.LineItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            // Ratings have a one to one required relationship with Customers
            // Customers have a one to many optional reverse relationship with Ratings
            entity.HasOne<Customer>(e => e.Customer)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            // Ratings have a one to one required relationship with Products
            // Products have a one to many optional revers relationship with Ratings
            entity.HasOne<Product>(e => e.Product)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });
        
    }
}