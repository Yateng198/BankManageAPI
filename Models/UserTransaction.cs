namespace RestAPITesting.Models
{
    public class UserTransaction
    {
        public int recordId { get; set; }
        public int userId { get; set; }
        public long cardNumber { get; set; }
        public string transactionType { get; set; }
        public string transactionTime { get; set; }
        public double transactionAmount { get; set; }

    }
}
