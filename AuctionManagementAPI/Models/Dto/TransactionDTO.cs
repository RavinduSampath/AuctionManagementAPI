namespace AuctionManagementAPI.Models.Dto
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public string PaymentMethod { get; set; }
    }

}
