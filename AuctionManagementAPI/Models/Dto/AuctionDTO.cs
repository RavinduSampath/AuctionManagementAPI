namespace AuctionManagementAPI.Models.Dto
{
    public class AuctionDTO
    {
        public int AuctionId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
        public int SellerId { get; set; }
    }

}
