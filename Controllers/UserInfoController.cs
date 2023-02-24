using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestAPIBank.Models;
using RestAPITesting.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace RestAPITesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }




        //Register new user
        [HttpPost]
        [Route("register")]
        public userInfoResponse register(UserInfo newUser)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.registerNewUser(con, newUser);
            return response;
        }

        public class LoginRequest
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        [HttpPost]
        [Route("login")]    
        public userInfoResponse Login([FromBody] LoginRequest request)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.Login(con, request.email, request.password);
            return response;
        }

        public class depositeRequest
        {
            public string amountAdding { get; set; }
            public string userCardNum { get; set; }
        }

        [HttpPost]
        [Route("deposit")]
        public userInfoResponse deposit([FromBody] depositeRequest request)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.deposit(con, request.amountAdding, request.userCardNum);
            return response;
        }

        [HttpPost]
        [Route("withdraw")]
        public userInfoResponse withdraw([FromBody] depositeRequest request)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.withdrawal(con, request.amountAdding, request.userCardNum);
            return response;
        }



    }
}
