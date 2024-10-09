namespace AuctionManagementAPI.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal EstimatedValue { get; set; }

        // Foreign Key to Auction
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
    }


}
