namespace RestAPITesting.Models
{
    public class UserInfo
    {
        public int userId { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime DOB { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string occupation { get; set; }
        public string addressStreet { get; set; }
        public string addressCity { get; set; }
        public string addressProvince { get; set; }
        public string addressCountry { get; set;}
        public string postalCode { get; set; }
        
    }
}
