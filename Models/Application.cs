using RestAPIBank.Models;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace RestAPITesting.Models
{
    public class Application
    {
        public Response GetAllCurrency(SqlConnection con)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("Select * from Currency", con);
            DataTable dt = new DataTable();
            List<Currency> listCurrency = new List<Currency>();
            da.Fill(dt);    
            if(dt.Rows.Count > 0)
            {
                for(int i = 0; i<dt.Rows.Count; i++) { 
                    Currency currency= new Currency();
                    currency.Date = (string)dt.Rows[i]["Date"];
                    currency.CAD = (double)dt.Rows[i]["CAD"];
                    currency.USD = (double)dt.Rows[i]["USD"];
                    currency.EUR = (double)dt.Rows[i]["EUR"];
                    currency.CNY = (double)dt.Rows[i]["CNY"];
                    currency.JPY = (double)dt.Rows[i]["JPY"];
                    currency.AUD = (double)dt.Rows[i]["AUD"];
                    currency.MXN = (double)dt.Rows[i]["MXN"];

                    listCurrency.Add(currency);
                }
            }
            if(listCurrency.Count > 0)
            {
                response.StatusCode= 200;
                response.StatusMessage = "Data Found";
                response.listCurrency = listCurrency;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listCurrency = null;
            }
            return response;
        }

        public Response GetAllCurrencyByID(SqlConnection con, String Date)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("Select * from Currency Where Date = '"+Date+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                
                    Currency currency = new Currency();           
                    currency.Date = (string)dt.Rows[0]["Date"];
                    currency.CAD = (double)dt.Rows[0]["CAD"];
                    currency.USD = (double)dt.Rows[0]["USD"];
                    currency.EUR = (double)dt.Rows[0]["EUR"];
                    currency.CNY = (double)dt.Rows[0]["CNY"];
                    currency.JPY = (double)dt.Rows[0]["JPY"];
                    currency.AUD = (double)dt.Rows[0]["AUD"];
                    currency.MXN = (double)dt.Rows[0]["MXN"];

                    response.StatusCode = 200;
                    response.StatusMessage = "Data Found";
                    response.currency = currency;


            }
            
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listCurrency = null;
            }
            return response;
        }

        public Response AddCurrency(SqlConnection con, Currency currency)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("Insert into Currency(Date, CAD, USD, EUR, CNY, JPY, AUD, MXN) Values('"
                + currency.Date + "','"+ currency.CAD +"', '"
                + currency.USD +"', '"+ currency.EUR + "', '"
                + currency.CNY + "', '" + currency.JPY + "', '"
                + currency.AUD + "', '" + currency.MXN
                + "') ", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Currency Added";
                


            }

            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Inserted";
                
            }
            return response;
        }

        public Response UpdateCurrency(SqlConnection con, Currency currency)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("Update Currency set CAD='" 
                + currency.CAD + "', USD='" + currency.USD + "', EUR='" +currency.EUR
                + currency.CNY + "', USD='" + currency.JPY + "', EUR='" + currency.AUD
                + currency.MXN + "' Where Date='"+ currency.Date +"'", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Employee Added";



            }

            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Inserted";

            }
            return response;
        }

        public Response DeleteCurrency(SqlConnection con, String Date)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("Delete from Currency Where Date='" + Date + "'", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Currency Deleted";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Currency Deleted";
            }
            
            return response;
        }
    }
}
