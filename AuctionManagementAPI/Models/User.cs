using System.Security.Cryptography;

namespace AuctionManagementAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Admin, Buyer, Seller
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<Auction> Auctions { get; set; }  // Seller's auctions
        public ICollection<Bid> Bids { get; set; }  // Buyer's bids
    }

}
