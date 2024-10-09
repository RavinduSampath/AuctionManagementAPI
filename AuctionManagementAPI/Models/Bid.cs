namespace AuctionManagementAPI.Models
{
    public class Bid
    {
        public int BidId { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidTime { get; set; }

        // Foreign Key to Auction
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }

        // Foreign Key to User (Bidder)
        public int BidderId { get; set; }
        public User Bidder { get; set; }
    }


}
