using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestAPIBank.Models;
using RestAPITesting.Models;
using System.Data.SqlClient;
using static RestAPITesting.Controllers.TransferController;

namespace RestAPITesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IConfiguration _configuration; // receive the connection state with sql server

        public TransactionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetAllTransaction")]


        public UserTransactionResponse GetUserTransaction()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            UserTransactionResponse response = new UserTransactionResponse();
            Application apl = new Application();
            response = apl.GetUserTransaction(con);
            return response;
        }

        [HttpGet("GetTransactionById")]

        public UserTransactionResponse GetTransactionById(int userId)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            UserTransactionResponse response = new UserTransactionResponse();
            Application apl = new Application();
            response = apl.GetTransactionById(con, userId);
            return response; ;
        }

        [HttpGet("GetTransactionThisMonth")]

        public UserTransactionResponse GetTransactionThisMonth(int userId)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            UserTransactionResponse response = new UserTransactionResponse();
            Application apl = new Application();
            response = apl.GetTransactionThisMonth(con, userId);
            return response; ;
        }

        [HttpGet("GetTransactionLastMonth")]

        public UserTransactionResponse GetTransactionLastMonth(int userId)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            UserTransactionResponse response = new UserTransactionResponse();
            Application apl = new Application();
            response = apl.GetTransactionLastMonth(con, userId);
            return response; ;
        }

        [HttpGet("GetTransactionIn3Month")]

        public UserTransactionResponse GetTransactionIn3Month(int userId)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            UserTransactionResponse response = new UserTransactionResponse();
            Application apl = new Application();
            response = apl.GetTransactionIn3Month(con, userId);
            return response; ;
        }

        [HttpGet("GetTransactionThisYear")]

        public UserTransactionResponse GetTransactionThisYear(int userId)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            UserTransactionResponse response = new UserTransactionResponse();
            Application apl = new Application();
            response = apl.GetTransactionThisYear(con, userId);
            return response; ;
        }


        [HttpGet]
        [Route("GoBack")]
        public userInfoResponse GoBackButton(int userId)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.GoBackButton(con, userId);
            return response;
        }
    }
}