using System.Security.Cryptography;

namespace AuctionManagementAPI.Models
{
    public class Auction
    {
        public int AuctionId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }

        // Foreign Key to User
        public int SellerId { get; set; }
        public User Seller { get; set; }

        // Bids
        public ICollection<Bid> Bids { get; set; }
    }


}
