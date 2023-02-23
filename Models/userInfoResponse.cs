using RestAPIBank.Models;

namespace RestAPITesting.Models
{
    public class userInfoResponse
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public UserInfo user { get; set; }

        public UserAccount accout { get; set; }  


        public List<UserInfo> users { get; set; }
    }
}
