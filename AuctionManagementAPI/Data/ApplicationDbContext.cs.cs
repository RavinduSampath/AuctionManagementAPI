using Microsoft.EntityFrameworkCore;
using AuctionManagementAPI.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // For Auction
        modelBuilder.Entity<Auction>()
            .Property(a => a.CurrentPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Auction>()
            .Property(a => a.StartingPrice)
            .HasColumnType("decimal(18,2)");

        // For Bid
        modelBuilder.Entity<Bid>()
            .Property(b => b.BidAmount)
            .HasColumnType("decimal(18,2)");

        // For Item
        modelBuilder.Entity<Item>()
            .Property(i => i.EstimatedValue)
            .HasColumnType("decimal(18,2)");

        // For Transaction
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18,2)");

        // Define the relationship for Bid to Auction
        modelBuilder.Entity<Bid>()
            .HasOne(b => b.Auction) // Navigation property to Auction
            .WithMany(a => a.Bids) // Navigation property in Auction (make sure Auction has a Bids collection)
            .HasForeignKey(b => b.AuctionId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Define the relationship for Bid to User (Bidder)
        modelBuilder.Entity<Bid>()
            .HasOne(b => b.Bidder) // Navigation property to User
            .WithMany(u => u.Bids) // Navigation property in User
            .HasForeignKey(b => b.BidderId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
    }


}
