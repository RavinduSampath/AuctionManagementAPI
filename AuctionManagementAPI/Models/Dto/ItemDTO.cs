namespace AuctionManagementAPI.Models.Dto
{
    public class ItemDTO
    {
        public int ItemId { get; set; } // Unique identifier for the item
        public string Name { get; set; } // Name of the item
        public string Description { get; set; } // Description of the item
        public string ImageUrl { get; set; } // URL to the item's image
        public decimal EstimatedValue { get; set; } // Estimated value of the item

        // Foreign Key to Auction
        public int AuctionId { get; set; } // Foreign key reference to the Auction
    }
}
