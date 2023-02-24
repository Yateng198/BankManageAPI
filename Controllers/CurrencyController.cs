using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestAPIBank.Models;
using RestAPITesting.Models;
using System.Data.SqlClient;

namespace RestAPITesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IConfiguration _configuration; // receive the connection state with sql server

        public CurrencyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetAllCurrency")]

        public Response GetAllCurrency()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.GetAllCurrency(con);
            return response;
        }

        [HttpGet]
        [Route("GetAllCurrencyByID/{Date}")]

        public Response GetAllCurrencyByID(String Date)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.GetAllCurrencyByID(con, Date);
            return response;
        }

        [HttpPost]
        [Route("AddCurrency")]
        public Response AddCurrency(Currency currency)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.AddCurrency(con, currency);
            return response;
        }


       

        [HttpPut]
        [Route("UpdateCurrency")]
        public Response UpdateCurrency(Currency currency)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.AddCurrency(con, currency);
            return response;
        }


        [HttpDelete]
        [Route("DeleteCurrency/{Date}")]
        public Response DeleteCurrency(string Date)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            Response response = new Response();
            Application apl = new Application();
            response = apl.DeleteCurrency(con, Date);
            return response;
        }
        
    }
}
