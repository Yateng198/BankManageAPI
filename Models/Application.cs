using Microsoft.AspNetCore.Mvc;
using RestAPIBank.Models;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

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
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Currency currency = new Currency();
                    currency.Date = dt.Rows[i]["Date"].ToString();
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
            if (listCurrency.Count > 0)
            {
                response.StatusCode = 200;
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
            SqlDataAdapter da = new SqlDataAdapter("Select * from Currency Where Date = '" + Date + "'", con);
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
                + currency.Date + "','" + currency.CAD + "', '"
                + currency.USD + "', '" + currency.EUR + "', '"
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
                + currency.CAD + "', USD='" + currency.USD + "', EUR='" + currency.EUR
                + currency.CNY + "', USD='" + currency.JPY + "', EUR='" + currency.AUD
                + currency.MXN + "' Where Date='" + currency.Date + "'", con);
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

        public userInfoResponse registerNewUser(SqlConnection con, UserInfo user)
        {
            userInfoResponse response = new userInfoResponse();
            con.Open();
            //Check if email is already exist in database
            string newEmail = user.email;
            string selectQuery = "SELECT COUNT(*) FROM UserInfo WHERE Email = @Email";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@Email", newEmail);
            int count = (int)selectCmd.ExecuteScalar();
            if (count > 0)
            {
                response.StatusCode = 100;
                response.StatusMessage = "Email already exists. Please enter a different email address!";

            }
            else
            {
                string newPwd = user.password.ToString();
                string pattern = @"^(?=.*[a-z].*[a-z])(?=.*[A-Z].*[A-Z])(?=.*\d.*\d)(?=.*[^a-zA-Z0-9].*[^a-zA-Z0-9]).{8,15}$";
                Regex regex = new Regex(pattern);
                if (regex.IsMatch(newPwd))
                {
                    // string password = pwd.Password.ToString();
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(newPwd);
                    byte[] hashBytes;
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        hashBytes = sha256.ComputeHash(passwordBytes);
                    }
                    string hashedPassword = Convert.ToBase64String(hashBytes);
                    string qury = "INSERT INTO UserInfo OUTPUT INSERTED.UserId values(@Password, @FName, @LName, @Dob, @Email, @PhoneNumber, @occupation, @street, @city, @province, @country, @postalcode)";
                    SqlCommand cmd = new SqlCommand(qury, con);

                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@FName", user.firstName);
                    cmd.Parameters.AddWithValue("@LName", user.lastName);
                    cmd.Parameters.AddWithValue("@Dob", user.DOB.ToShortDateString());
                    cmd.Parameters.AddWithValue("@Email", user.email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.phoneNumber);
                    cmd.Parameters.AddWithValue("@occupation", user.occupation);
                    cmd.Parameters.AddWithValue("@street", user.addressStreet);
                    cmd.Parameters.AddWithValue("@city", user.addressCity);
                    cmd.Parameters.AddWithValue("@province", user.addressProvince);
                    cmd.Parameters.AddWithValue("@country", user.addressCountry);
                    cmd.Parameters.AddWithValue("@postalcode", user.postalCode);

                    // Execute the query and retrieve the newly created UserId
                    int newUserId = (int)cmd.ExecuteScalar();
                    //Randomlly get an account number for the new user
                    Random rand = new Random();
                    //long accountNumber = (long)(rand.NextDouble() * (9832789 - 3) + rand.NextDouble() * (89732 - 298)) * 100;
                    int digits = 10;
                    long accountNumber = long.Parse(string.Join("", Enumerable.Range(1, digits - 1).Select(_ => rand.Next(10).ToString())) + rand.Next(1, 10).ToString());


                    string accountQuery = "Insert Into UserAccount values (@UserId, @CardNumber, 0.0)";
                    SqlCommand accountCmd = new SqlCommand(accountQuery, con);
                    accountCmd.Parameters.AddWithValue("@UserId", newUserId);
                    accountCmd.Parameters.AddWithValue("@CardNumber", accountNumber);
                    int i = accountCmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "New Customer Registered Successfully, click ok and go back to log in page!";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "New Customer Registration Failed, Check your information and try again please!";
                    }
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Password must contain: 2 Uppercase letters, 2 Lowercase letters, 2 digits and 2 special charactors, length 8-15!";
                }
            }
            return response;
        }

        //Login method
        public userInfoResponse Login(SqlConnection con, string email, string pwd)
        {
            userInfoResponse response = new userInfoResponse();
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select count(1) from UserInfo where Email = @Email and Password = @Pwd COLLATE SQL_Latin1_General_CP1_CS_AS", con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Pwd", pwd);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    SqlCommand cmd1 = new SqlCommand("select UserId, Password, F_Name, L_Name, Date_Of_Birth, Email, Mobile, Occupation, Address_Street, Address_City, Address_Province, Address_Country, Postcode from UserInfo where email = @Email COLLATE SQL_Latin1_General_CP1_CS_AS", con);
                    cmd1.Parameters.AddWithValue("@Email", email);
                    SqlDataReader userInfoReader = cmd1.ExecuteReader();
                    UserInfo userLogIn = new UserInfo();
                    while (userInfoReader.Read())
                    {
                        userLogIn.userId = (int)userInfoReader.GetValue(0);
                        userLogIn.password = (string)userInfoReader.GetValue(1);
                        userLogIn.firstName = userInfoReader.GetValue(2).ToString();
                        userLogIn.lastName = userInfoReader.GetValue(3).ToString();
                        userLogIn.DOB = userInfoReader.GetDateTime(4).Date;
                        userLogIn.email = userInfoReader.GetValue(5).ToString();
                        userLogIn.phoneNumber = userInfoReader.GetValue(6).ToString();
                        userLogIn.occupation = userInfoReader.GetValue(7).ToString();
                        userLogIn.addressStreet = userInfoReader.GetValue(8).ToString();
                        userLogIn.addressCity = userInfoReader.GetValue(9).ToString();
                        userLogIn.addressProvince = userInfoReader.GetValue(10).ToString();
                        userLogIn.addressCountry = userInfoReader.GetValue(11).ToString();
                        userLogIn.postalCode = userInfoReader.GetValue(12).ToString();
                    }


                    userInfoReader.Close();

                    SqlCommand cmd2 = new SqlCommand("select CardNumber, Balance from UserAccount where UserId = @id", con);
                    cmd2.Parameters.AddWithValue("@id", userLogIn.userId);
                    SqlDataReader userAccountReader = cmd2.ExecuteReader();
                    UserAccount accoutLogIn = new UserAccount();

                    accoutLogIn.userId = userLogIn.userId;
                    while (userAccountReader.Read())
                    {
                        accoutLogIn.CardNumber = userAccountReader.GetInt64(0);
                        accoutLogIn.Balance = userAccountReader.GetDouble(1);
                    }

                    userAccountReader.Close();


                    response.user = userLogIn;
                    response.accout = accoutLogIn;
                    response.StatusCode = 200;
                    response.StatusMessage = "Log in successed!";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Please Check Your Email & Password, and Try Again!";
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = "Please Check Your Email & Password, and Try Again!";
            }

            return response;
        }

        public userInfoResponse deposit(SqlConnection con, string amount, string usercardnumber)
        {
            con.Open();
            userInfoResponse userInfoResponse = new userInfoResponse();

            string query = "UPDATE UserAccount SET Balance = Balance + @depositAmount WHERE CardNumber = @accountNumber";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@depositAmount", amount);
            cmd.Parameters.AddWithValue("@accountNumber", usercardnumber);
            cmd.ExecuteNonQuery();

            // Retrieve the new balance value from the database
            query = "SELECT UserId, Balance FROM UserAccount WHERE CardNumber = @accountNumber";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@accountNumber", usercardnumber);
            SqlDataReader reader = cmd.ExecuteReader();
            int loggedUserId = 0;
            double newAmount = 0;
            while (reader.Read())
            {
                loggedUserId = reader.GetInt32(0);
                newAmount = reader.GetDouble(1);
            }
            reader.Close();
            // amount = newAmount.ToString() + "$";

            //Update deposit record into database
            cmd = new SqlCommand("Insert into UserTransaction values (@userid, @cardNum, @type, @time, @amount)", con);
            DateTime currentDateTime = DateTime.Now;
            cmd.Parameters.AddWithValue("@userid", loggedUserId.ToString());
            cmd.Parameters.AddWithValue("@cardNum", usercardnumber);
            cmd.Parameters.AddWithValue("@type", "Deposit");
            cmd.Parameters.AddWithValue("@time", currentDateTime);
            cmd.Parameters.AddWithValue("@amount", amount);
            int i = cmd.ExecuteNonQuery();
            UserAccount acc = new UserAccount();
            acc.userId = loggedUserId;
            acc.Balance = newAmount;


            if (i > 0)
            {
                userInfoResponse.accout = acc;
                userInfoResponse.StatusCode = 200;
                userInfoResponse.StatusMessage = "You have deposited " + amount + "$ successfully!";
            }
            else
            {
                userInfoResponse.StatusCode = 100;
                userInfoResponse.StatusMessage = "Deposited Failed!";

            }
            con.Close();
            return userInfoResponse;

        }

        public userInfoResponse withdrawal(SqlConnection con, string amount, string usercardnumber)
        {
            con.Open();
            userInfoResponse userInfoResponse = new userInfoResponse();

            string query = "UPDATE UserAccount SET Balance = Balance - @windrawalAmount WHERE CardNumber = @accountNumber";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@windrawalAmount", amount);
            cmd.Parameters.AddWithValue("@accountNumber", usercardnumber);
            cmd.ExecuteNonQuery();

            // Retrieve the new balance value from the database
            query = "SELECT UserId, Balance FROM UserAccount WHERE CardNumber = @accountNumber";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@accountNumber", usercardnumber);
            SqlDataReader reader = cmd.ExecuteReader();
            int loggedUserId = 0;
            double newAmount = 0;
            while (reader.Read())
            {
                loggedUserId = reader.GetInt32(0);
                newAmount = reader.GetDouble(1);
            }
            reader.Close();

            //Update deposit record into database
            cmd = new SqlCommand("Insert into UserTransaction values (@userid, @cardNum, @type, @time, @amount)", con);
            DateTime currentDateTime = DateTime.Now;
            cmd.Parameters.AddWithValue("@userid", loggedUserId.ToString());
            cmd.Parameters.AddWithValue("@cardNum", usercardnumber);
            cmd.Parameters.AddWithValue("@type", "Withdrawal");
            cmd.Parameters.AddWithValue("@time", currentDateTime);
            cmd.Parameters.AddWithValue("@amount", (0-double.Parse(amount)).ToString());
            int i = cmd.ExecuteNonQuery();
            UserAccount acc = new UserAccount();
            acc.userId = loggedUserId;
            acc.Balance = newAmount;


            if (i > 0)
            {
                userInfoResponse.accout = acc;
                userInfoResponse.StatusCode = 200;
                userInfoResponse.StatusMessage = "You have withdrawn " + amount + "$ successfully!";
            }
            else
            {
                userInfoResponse.StatusCode = 100;
                userInfoResponse.StatusMessage = "Withdrawn Failed!";

            }
            con.Close();
            return userInfoResponse;

        }

        public userInfoResponse transferByEmail(SqlConnection con, string amount, string email)
        {
            con.Open();
            userInfoResponse response = new userInfoResponse();
            SqlCommand cmd = new SqlCommand("select count(1) from UserInfo where Email = @email COLLATE SQL_Latin1_General_CP1_CS_AS", con);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@email", email);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 1)
            {
                //Add check if sender and receiver is the same account
                cmd = new SqlCommand("select UserId, F_Name, L_Name from UserInfo where Email = @email COLLATE SQL_Latin1_General_CP1_CS_AS", con);

                cmd.Parameters.AddWithValue("@email", email);
                SqlDataReader reader = cmd.ExecuteReader();
                int userId = 0;
                string firstName = "";
                string lastName = "";
                while (reader.Read())
                {
                    userId = reader.GetInt32(0);
                    firstName = reader.GetString(1);
                    lastName = reader.GetString(2);
                }
                reader.Close();
                UserInfo user = new UserInfo();
                user.userId = userId;
                user.firstName = firstName;
                user.lastName = lastName;
                response.user = user;
                response.StatusCode = 200;
                response.StatusMessage = "You are Making a Tranfer of Amount: " + amount + "$\r\n" + "To: " + firstName + " " + lastName +
                               "\r\nConfirm or Cancel?";
                con.Close();
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No user found! Check the email entered and try again please";
                con.Close();
                return response;
            }
            con.Close();
            return response;
        }

        //Transfer by user account number
        public userInfoResponse transferByCardNumber(SqlConnection con, string amount, string cardNumber)
        {
            con.Open();
            userInfoResponse response = new userInfoResponse();
            //Check if there is any user corresponding this card number
            SqlCommand cmd = new SqlCommand("select UserId from UserAccount where CardNumber = @cardnumber", con);
            cmd.Parameters.AddWithValue("@cardnumber", cardNumber);
            var checkID = cmd.ExecuteScalar();
            if (checkID != null)
            {
                //Add check if sender and receiver is the same account
                try
                {
                    int userId = (int)cmd.ExecuteScalar();
                    string firstName = "";
                    string lastName = "";
                    if (userId != 0)
                    {
                        //Take out the user first name and last name for confirmation button click
                        cmd = new SqlCommand("select F_Name, L_Name from UserInfo where UserId = @userid", con);
                        cmd.Parameters.AddWithValue("@userid", userId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            firstName = reader.GetString(0);
                            lastName = reader.GetString(1);
                        }
                        reader.Close();
                        UserInfo user = new UserInfo();
                        user.userId = userId;
                        user.firstName = firstName;
                        user.lastName = lastName;
                        response.user = user;
                        response.StatusCode = 200;
                        response.StatusMessage = "You are Making a Tranfer of Amount: " + amount + "\r\n" + "To: " + firstName + " " + lastName +
                                       "\r\nConfirm or Cancel?";
                        con.Close();
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "No Account found! Check the Account Nuber entered and try again please";
                        con.Close();
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Please enter a valide Account Number and try again!";
                    con.Close();
                    return response;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Account found! Check the Account Nuber entered and try again please!";
                con.Close();
                return response;
            }
            con.Close();
            return response;
        }
        //Confirm Button Click method
        public userInfoResponse confirmButton(SqlConnection con, string senderCurrentBalance, string recipientUserId, string amountTrans, string senderEmail)
        {
            con.Open();
            userInfoResponse response = new userInfoResponse();
            //update receivers balance to database
            SqlCommand cmd = new SqlCommand("Select Balance from UserAccount where UserId = @ID", con);
            cmd.Parameters.AddWithValue("@ID", recipientUserId);
            double receiverCurrentBalance = (double)cmd.ExecuteScalar() + double.Parse(amountTrans);
            con.Close();
            con.Open();
            cmd = new SqlCommand("UPDATE UserAccount SET Balance = @balance WHERE UserId = @ID", con);
            cmd.Parameters.AddWithValue("@balance", receiverCurrentBalance);
            cmd.Parameters.AddWithValue("@ID", recipientUserId);
            cmd.ExecuteNonQueryAsync();
            con.Close();
            con.Open();

            //update senders balance to database
            cmd = new SqlCommand("Select UserId from UserInfo where Email = @email COLLATE SQL_Latin1_General_CP1_CS_AS", con);
            cmd.Parameters.AddWithValue("@email", senderEmail);
            int senderUserId = (int)cmd.ExecuteScalar();
            double senderNewAmount = double.Parse(senderCurrentBalance) - double.Parse(amountTrans);
            con.Close();
            con.Open();
            cmd = new SqlCommand("UPDATE UserAccount SET Balance = @newBalance WHERE UserId = @ID", con);
            cmd.Parameters.AddWithValue("@newBalance", senderNewAmount);
            cmd.Parameters.AddWithValue("@ID", senderUserId);
            cmd.ExecuteNonQueryAsync();
            con.Close();
            con.Open();

            //save this transaction into dataase, for both sneder and receiver

            //Get sender's card number
            cmd = new SqlCommand("select CardNumber from UserAccount where UserId = @senderId", con);
            cmd.Parameters.AddWithValue("@senderId", senderUserId);
            SqlDataReader reader = cmd.ExecuteReader();
            long senderCardNumber = 0;
            //long receiverCardNumber = 0;
            while (reader.Read())
            {
                senderCardNumber = long.Parse(reader.GetValue(0).ToString());
            }
            reader.Close();
            con.Close();
            //Get receipient card number
            con.Open();
            cmd = new SqlCommand("select CardNumber from UserAccount where UserId = @receiverId", con);
            //cmd.Parameters.AddWithValue("@senderId", senderUserId);
            cmd.Parameters.AddWithValue("@receiverId", recipientUserId);
            reader = cmd.ExecuteReader();
           // long senderCardNumber = 0;
            long receiverCardNumber = 0;
            while (reader.Read())
            {
                receiverCardNumber = long.Parse(reader.GetValue(0).ToString());
            }
            reader.Close();
            con.Close();
            con.Open();
            //Retrieve sender personal information
            cmd = new SqlCommand("select F_Name, L_Name, Date_Of_Birth, Mobile, Address_Country from UserInfo where UserId = @id ", con);
            cmd.Parameters.AddWithValue("@id", senderUserId);
            reader = cmd.ExecuteReader();
            UserInfo senderUserInfo = new UserInfo();
            senderUserInfo.firstName = string.Empty;
            while (reader.Read())
            {
                senderUserInfo.firstName = reader.GetString(0);
                senderUserInfo.lastName = reader.GetString(1);
                senderUserInfo.DOB = reader.GetDateTime(2);
                senderUserInfo.phoneNumber = reader.GetString(3);
                senderUserInfo.addressCountry = reader.GetString(4);
            }
            reader.Close();
            con.Close();
            con.Open();
            //Insert record into sender database
            cmd = new SqlCommand("insert into UserTransaction values (@Userid, @CardNumber, @TransactionType, @TransactionTime, @TransactionAmount)", con);
            cmd.Parameters.AddWithValue("@Userid", senderUserId);
            cmd.Parameters.AddWithValue("@CardNumber", senderCardNumber);
            cmd.Parameters.AddWithValue("@TransactionType", "Transfer OUT");
            DateTime currrentDateTime = DateTime.Now;
            cmd.Parameters.AddWithValue("@TransactionTime", currrentDateTime);
            cmd.Parameters.AddWithValue("@TransactionAmount", (0 - double.Parse(amountTrans)).ToString());
            cmd.ExecuteNonQueryAsync();
            con.Close();
            con.Open();

            //Insert record into receiver database
            cmd = new SqlCommand("insert into UserTransaction values (@Userid, @CardNumber, @TransactionType, @TransactionTime, @TransactionAmount)", con);
            cmd.Parameters.AddWithValue("@Userid", recipientUserId);
            cmd.Parameters.AddWithValue("@CardNumber", receiverCardNumber);
            cmd.Parameters.AddWithValue("@TransactionType", "Transfer IN");
            cmd.Parameters.AddWithValue("@TransactionTime", currrentDateTime);
            cmd.Parameters.AddWithValue("@TransactionAmount", amountTrans);
            cmd.ExecuteNonQueryAsync();

            UserAccount senderUserAccount = new UserAccount();
            senderUserAccount.Balance = senderNewAmount;
            senderUserAccount.CardNumber = senderCardNumber;

            //Setup response object
            response.StatusCode = 200;
            response.StatusMessage = "Successed!";
            response.user = senderUserInfo;
            response.accout = senderUserAccount;
            con.Close();
            return response;
        }

        public userInfoResponse cacelButton(SqlConnection con, string email)
        {
            con.Open();
            userInfoResponse response = new userInfoResponse();
            //Retrieve original user information for back to myAccount window
            SqlCommand cmd = new SqlCommand("select F_Name, L_Name, Date_Of_Birth, Mobile, Address_Country, UserId from UserInfo where Email = @email COLLATE SQL_Latin1_General_CP1_CS_AS ", con);
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataReader reader = cmd.ExecuteReader();
            UserInfo user = new UserInfo();
            while (reader.Read())
            {
                user.firstName = reader.GetString(0);
                user.lastName = reader.GetString(1);
                user.DOB = reader.GetDateTime(2);
                user.phoneNumber = reader.GetString(3);
                user.addressCountry = reader.GetString(4);
                //ID will use in next query to retrieve account information
                user.userId = reader.GetInt32(5);
            }
            reader.Close();
            con.Close();
            con.Open();
            //Retrieve updated user account information for myAccount window
            cmd = new SqlCommand("select CardNumber, Balance from UserAccount where UserId = @id", con);
            cmd.Parameters.AddWithValue("@id", user.userId.ToString());
            reader = cmd.ExecuteReader();
            UserAccount account = new UserAccount();
            while (reader.Read())
            {
                account.CardNumber = reader.GetInt64(0);
                account.Balance = reader.GetDouble(1);

            }
            reader.Close();

            con.Close();

            response.StatusMessage = string.Empty;
            response.StatusCode = 200;
            response.user = user;
            response.accout = account;
            return response;
        }

        public UserTransactionResponse GetUserTransaction(SqlConnection con)
        {
            UserTransactionResponse response = new UserTransactionResponse();
            SqlDataAdapter da = new SqlDataAdapter("Select * from UserTransaction", con);
            DataTable dt = new DataTable();
            List<UserTransaction> listTransaction = new List<UserTransaction>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserTransaction transaction = new UserTransaction();
                    transaction.recordId = (int)dt.Rows[i]["recordId"];
                    transaction.userId = (int)dt.Rows[i]["userId"];
                    transaction.cardNumber = (long)dt.Rows[i]["cardNumber"];
                    transaction.transactionType = dt.Rows[i]["transactionType"].ToString();
                    transaction.transactionTime = ((DateTime)dt.Rows[i]["transactionTime"]).ToString();
                    transaction.transactionAmount = Convert.ToDouble(dt.Rows[i]["transactionAmount"]);


                    listTransaction.Add(transaction);
                }
            }
            if (listTransaction.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User Transaction Found";
                response.listTransaction = listTransaction;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listTransaction = null;
            }
            return response;
        }

        public UserTransactionResponse GetTransactionById(SqlConnection con, int userId)
        {
            UserTransactionResponse response = new UserTransactionResponse();
            SqlCommand cmd = new SqlCommand("SELECT * FROM UserTransaction WHERE userId = @userId", con);
            cmd.Parameters.AddWithValue("@userId", userId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<UserTransaction> listTransaction = new List<UserTransaction>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserTransaction transaction = new UserTransaction();
                    transaction.recordId = (int)dt.Rows[i]["recordId"];
                    transaction.userId = (int)dt.Rows[i]["userId"];
                    transaction.cardNumber = (long)dt.Rows[i]["cardNumber"];
                    transaction.transactionType = dt.Rows[i]["transactionType"].ToString();
                    transaction.transactionTime = ((DateTime)dt.Rows[i]["transactionTime"]).ToString("yyyy-MM-dd");
                    transaction.transactionAmount = (double)dt.Rows[i]["transactionAmount"];

                    listTransaction.Add(transaction);
                }
            }
            if (listTransaction.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User Transaction Found";
                response.listTransaction = listTransaction;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listTransaction = null;
            }
            return response;
        }

        public UserTransactionResponse GetTransactionThisMonth(SqlConnection con, int userId)
        {
            UserTransactionResponse response = new UserTransactionResponse();
            DateTime currentDate = DateTime.Now;
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM UserTransaction WHERE userId = '" + userId + "' AND MONTH(transactionTime) = " + currentDate.Month, con);
            DataTable dt = new DataTable();
            List<UserTransaction> listTransaction = new List<UserTransaction>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserTransaction transaction = new UserTransaction();
                    transaction.recordId = (int)dt.Rows[i]["recordId"];
                    transaction.userId = (int)dt.Rows[i]["userId"];
                    transaction.cardNumber = (long)dt.Rows[i]["cardNumber"];
                    transaction.transactionType = dt.Rows[i]["transactionType"].ToString();
                    transaction.transactionTime = ((DateTime)dt.Rows[i]["transactionTime"]).ToString("yyyy-MM-dd");
                    transaction.transactionAmount = (double)dt.Rows[i]["transactionAmount"];

                    listTransaction.Add(transaction);
                }
            }
            if (listTransaction.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User Transaction Found";
                response.listTransaction = listTransaction;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listTransaction = null;
            }
            return response;
        }

        public UserTransactionResponse GetTransactionLastMonth(SqlConnection con, int userId)
        {
            UserTransactionResponse response = new UserTransactionResponse();
            SqlCommand cmd = new SqlCommand("SELECT * FROM UserTransaction WHERE userId = @userId AND MONTH(transactionTime) = MONTH(DATEADD(month, -1, GETDATE())) AND YEAR(transactionTime) = YEAR(DATEADD(month, -1, GETDATE()))", con);
            cmd.Parameters.AddWithValue("@userId", userId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<UserTransaction> listTransaction = new List<UserTransaction>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserTransaction transaction = new UserTransaction();
                    transaction.recordId = (int)dt.Rows[i]["recordId"];
                    transaction.userId = (int)dt.Rows[i]["userId"];
                    transaction.cardNumber = (long)dt.Rows[i]["cardNumber"];
                    transaction.transactionType = dt.Rows[i]["transactionType"].ToString();
                    transaction.transactionTime = ((DateTime)dt.Rows[i]["transactionTime"]).ToString("yyyy-MM-dd");
                    transaction.transactionAmount = (double)dt.Rows[i]["transactionAmount"];

                    listTransaction.Add(transaction);
                }
            }
            if (listTransaction.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User Transaction Found";
                response.listTransaction = listTransaction;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listTransaction = null;
            }
            return response;
        }

        public UserTransactionResponse GetTransactionIn3Month(SqlConnection con, int userId)
        {
            UserTransactionResponse response = new UserTransactionResponse();
            SqlCommand cmd = new SqlCommand("SELECT * FROM UserTransaction WHERE userId = @userId AND transactionTime >= DATEADD(month, -3, GETDATE()) AND transactionTime <= GETDATE()", con);
            cmd.Parameters.AddWithValue("@userId", userId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<UserTransaction> listTransaction = new List<UserTransaction>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserTransaction transaction = new UserTransaction();
                    transaction.recordId = (int)dt.Rows[i]["recordId"];
                    transaction.userId = (int)dt.Rows[i]["userId"];
                    transaction.cardNumber = (long)dt.Rows[i]["cardNumber"];
                    transaction.transactionType = dt.Rows[i]["transactionType"].ToString();
                    transaction.transactionTime = ((DateTime)dt.Rows[i]["transactionTime"]).ToString("yyyy-MM-dd");
                    transaction.transactionAmount = (double)dt.Rows[i]["transactionAmount"];

                    listTransaction.Add(transaction);
                }
            }
            if (listTransaction.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User Transaction Found";
                response.listTransaction = listTransaction;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listTransaction = null;
            }
            return response;
        }

        public UserTransactionResponse GetTransactionThisYear(SqlConnection con, int userId)
        {
            UserTransactionResponse response = new UserTransactionResponse();
            SqlCommand cmd = new SqlCommand("SELECT * FROM UserTransaction WHERE userId = @userId AND YEAR(transactionTime) = YEAR(GETDATE())", con);
            cmd.Parameters.AddWithValue("@userId", userId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            List<UserTransaction> listTransaction = new List<UserTransaction>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserTransaction transaction = new UserTransaction();
                    transaction.recordId = (int)dt.Rows[i]["recordId"];
                    transaction.userId = (int)dt.Rows[i]["userId"];
                    transaction.cardNumber = (long)dt.Rows[i]["cardNumber"];
                    transaction.transactionType = dt.Rows[i]["transactionType"].ToString();
                    transaction.transactionTime = ((DateTime)dt.Rows[i]["transactionTime"]).ToString("yyyy-MM-dd");
                    transaction.transactionAmount = (double)dt.Rows[i]["transactionAmount"];

                    listTransaction.Add(transaction);
                }
            }
            if (listTransaction.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User Transaction Found";
                response.listTransaction = listTransaction;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listTransaction = null;
            }
            return response;
        }

        public userInfoResponse GoBackButton(SqlConnection con, int userId)
        {
            con.Open();
            userInfoResponse response = new userInfoResponse();
            //Retrieve original user information for back to myAccount window
            SqlCommand cmd = new SqlCommand("select F_Name, L_Name, Date_Of_Birth, Mobile, Address_Country, UserId, Email from UserInfo where UserId = @userId", con);
            cmd.Parameters.AddWithValue("@userId", userId);
            SqlDataReader reader = cmd.ExecuteReader();
            UserInfo user = new UserInfo();
            while (reader.Read())
            {
                user.firstName = reader.GetString(0);
                user.lastName = reader.GetString(1);
                user.DOB = reader.GetDateTime(2);
                user.phoneNumber = reader.GetString(3);
                user.addressCountry = reader.GetString(4);
                //ID will use in next query to retrieve account information
                user.userId = reader.GetInt32(5);
                user.email = reader.GetString(6);
            }
            reader.Close();
            con.Close();
            con.Open();
            //Retrieve updated user account information for myAccount window
            cmd = new SqlCommand("select CardNumber, Balance from UserAccount where UserId = @id", con);
            cmd.Parameters.AddWithValue("@id", user.userId.ToString());
            reader = cmd.ExecuteReader();
            UserAccount account = new UserAccount();
            while (reader.Read())
            {
                account.CardNumber = reader.GetInt64(0);
                account.Balance = reader.GetDouble(1);

            }
            reader.Close();

            con.Close();

            response.StatusMessage = string.Empty;
            response.StatusCode = 200;
            response.user = user;
            response.accout = account;
            return response;
        }


    }
}






