using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPITesting.Models;
using System.Data.SqlClient;
using static RestAPITesting.Controllers.UserInfoController;

namespace RestAPITesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TransferController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class transferEmailRequest
        {
            public string amountTran { get; set; }
            public string emailAddress { get; set; }
        }

        //Transfer by Email address
        [HttpPost]
        [Route("eamil")]
        public userInfoResponse transferByEmail([FromBody] transferEmailRequest request)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.transferByEmail(con, request.amountTran, request.emailAddress);
            return response;
        }

        public class transferCardRequest
        {
            public string amountTran { get; set; }
            public string cardNumber { get; set; }
        }

        //Transfer by card number
        [HttpPost]
        [Route("card")]
        public userInfoResponse transferByCardNumber([FromBody] transferCardRequest request)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.transferByCardNumber(con, request.amountTran, request.cardNumber);
            return response;
        }


        public class confirmRequest
        {
            public string senderCurrentBalance { get; set; }
            public string recipientUserId { get; set; }
            public string amountTrans { get; set; }
            public string senderEmail { get; set; }
        }


        //Confirm transfer and save records into database
        [HttpPost]
        [Route("confirm")]
        public userInfoResponse confirmTransfer([FromBody] confirmRequest request)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.confirmButton(con, request.senderCurrentBalance, request.recipientUserId, request.amountTrans, request.senderEmail);
            return response;
        }

        public class cancelRequst
        {
            public string email { get; set; }
        }

        //Confirm transfer and save records into database
        [HttpPost]
        [Route("cancel")]
        public userInfoResponse cancelButton([FromBody] cancelRequst request)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("CurrencyCon").ToString());
            userInfoResponse response = new userInfoResponse();
            Application apl = new Application();
            response = apl.cacelButton(con, request.email);
            return response;
        }

    }
}
