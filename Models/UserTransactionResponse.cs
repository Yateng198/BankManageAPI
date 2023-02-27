namespace RestAPITesting.Models
{
    public class UserTransactionResponse
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public UserTransaction userTransaction { get; set; }

        public UserAccount account { get; set; }


        public List<UserTransaction> listTransaction { get; set; }
    }
}
