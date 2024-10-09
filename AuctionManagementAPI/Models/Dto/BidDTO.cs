namespace AuctionManagementAPI.Models.Dto
{
    public class BidDTO
    {
        public int BidId { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidTime { get; set; }
        public int AuctionId { get; set; }
        public int BidderId { get; set; }
    }

}
