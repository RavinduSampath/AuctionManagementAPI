namespace AuctionManagementAPI.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        // Foreign Key to User
        public int UserId { get; set; }
        public User User { get; set; }

        // Foreign Key to Auction
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }

        public string PaymentMethod { get; set; }  // E.g., PayPal, Stripe
    }

}
