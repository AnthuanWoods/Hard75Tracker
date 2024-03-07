using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
//using System.Web.Http.Cors;
using Hard75API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Json;
using Serilog;
using System;

namespace Hard75API.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserAccountController
    {
        public readonly IConfiguration _configuration;
        private readonly ILogger<UserAccountController> _logger;
        

        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public UserAccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/mylog.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }

        [HttpPost]
        [Route("AttemptSignin")]
        public Hard75Shared.Response SigninAttempt([FromBody] Hard75Shared.UserAccount prevuser)
        {
            Log.Information("SigninAttempt:Starting");
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    Log.Information("SigninAttempt:Connected to the Database");
                    using (SqlCommand sqlcmd = new SqlCommand("Select pwd, salt from UserAccount where email = @email", con))
                    {
                        sqlcmd.Parameters.AddWithValue("@email", prevuser.email);
                        Log.Information("SigninAttempt:Executing Select query for " + prevuser.email);
                        using (var results = sqlcmd.ExecuteReader())
                        {
                            if (results.HasRows)
                            {
                                Log.Information("SigninAttempt:User Record Found"); 
                                results.Read();
                                response.statusCode = 100;
                                string pwd = results.GetString(0).ToString();
                                string saltstr = results.GetString(1).ToString();
                                byte[] salt = Encoding.UTF8.GetBytes(results.GetValue(1).ToString());
                                Log.Information("SigninAttempt:Validating Login");
                                if (VerifyPassword(prevuser.pwd, pwd, saltstr))
                                {
                                    Log.Information("SigninAttempt:User Validated"); 
                                    response.statusCode = 100;
                                    response.message = "User Login Successful.";
                                    return response;
                                }
                                else
                                {
                                    Log.Information("SigninAttempt:User Login Incorrect"); 
                                    response.statusCode = 002;
                                    response.message = "User Email Address or Password is incorrect.";
                                    return response;
                                }
                            }
                            else
                            {
                                Log.Information("SigninAttempt:User does not exist"); 
                                response.statusCode = 001;
                                response.message = "User Email Address or Password is incorrect.";
                                return response;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Information("SigninAttempt:"+ex.ToString());
                response.statusCode = 999;
                response.message = "Sign In Attempt Error:"+ex.ToString();
                return response;
            }
        }
        
        [HttpGet]
        [Route("ViewUser")]
        public string GetUserAccount(string email)
        {
            Log.Information("GetUserAccount:Starting");
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    Log.Information("GetUserAccount:Connection to Database Open"); 
                    using (SqlCommand sqlcmd = new SqlCommand("Select * from UserAccount where email = @email", con))
                    {
                        sqlcmd.Parameters.AddWithValue("@email", email);
                        Log.Information("GetUserAccount:Executing Select Query"); 
                        using (var results = sqlcmd.ExecuteReader())
                        {
                            if (results.HasRows)
                            {
                                Log.Information("GetUserAccount:Found SQL Record");
                                results.Read();
                                response.statusCode = 101;
                                response.message = "{userID:" + results.GetInt32(0).ToString() + "}";
                                return JsonConvert.SerializeObject(response);
                            }
                            else
                            {
                                Log.Information("GetUserAccount:User does not exist"); 
                                response.statusCode = 003;
                                response.message = "User Email Address or Password is incorrect.";
                                return JsonConvert.SerializeObject(response);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 998;
                Log.Information("GetUserAccount:"+ ex.ToString());
                response.message = "View User Error:" + ex.ToString();
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("CreateUser")]
        public string CreateUserAccount([FromBody] Hard75Shared.UserAccount newuser)
        {
            Log.Information("CreateUserAccount:Started");
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                var hash = HashPasword(newuser.pwd, out var salt);
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    Log.Information("CreateUserAccount:Connected to Database");
                    using (SqlCommand sqlUserCheck = new SqlCommand("Select userID from UserAccount where email = @email and active = 1", con))
                    {
                        sqlUserCheck.Parameters.AddWithValue("@email", newuser.email);
                        Log.Information("CreateUserAccount:Executing Check for unique email.");
                        using (var results = sqlUserCheck.ExecuteReader())
                        {
                            if (results.HasRows)
                            {
                                Log.Information("CreateUserAccount:"+ newuser.email+" Address exists");
                                response.statusCode = 001;
                                response.message = "User Email Address already exists.";
                                return JsonConvert.SerializeObject(response);
                            }
                            else
                            {
                                Log.Information("CreateUserAccount:" + newuser.email + " is new");
                            }
                        }
                    }
                    Log.Information("CreateUserAccount:Creating User Record");
                    using (SqlCommand sqlcmd = new SqlCommand("Insert into UserAccount(email,pwd,salt,active) values(@email,@pwd,@salt,1)", con))
                    {
                        sqlcmd.Parameters.AddWithValue("@email", newuser.email);
                        sqlcmd.Parameters.AddWithValue("@pwd", hash);
                        sqlcmd.Parameters.AddWithValue("@salt", salt);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = sqlcmd;
                        adapter.InsertCommand.ExecuteNonQuery();
                        response.statusCode = 102;
                        response.message = "User successfully created.";
                        Log.Information("CreateUserAccount:User successfully created");
                        return JsonConvert.SerializeObject(response);
                    }
                }
            } catch(Exception ex) {
                response.statusCode = 997;
                Log.Information("CreateUserAccount:"+ ex.ToString());
                response.message = "Create User Error:" + ex.ToString();
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("ModifyUser")]
        public string UpdateUserAccount([FromBody] Hard75Shared.UserAccount newuser)
        {
            Log.Information("UpdateUserAccount:Function Start");
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                var hash = HashPasword(newuser.pwd, out var salt);
                Log.Information("UpdateUserAccount:Connecting to database");
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    Log.Information("UpdateUserAccount:Executing Select Query");
                    using (SqlCommand sqlcmd2 = new SqlCommand("Select userID from UserAccount where email = @email and active = 1", con))
                    {
                        sqlcmd2.Parameters.AddWithValue("@email", newuser.email);
                        using (var results = sqlcmd2.ExecuteReader())
                        {
                            if (results.HasRows)
                            {
                                Log.Information("UpdateUserAccount:Found User Account");
                                SqlDataAdapter adapter2 = new SqlDataAdapter("Update UserAccount set firstName = @fname, lastName = @lname, pwd = @pwd, salt = @salt, active = 1 where email = @email", con);
                                adapter2.UpdateCommand.Parameters.AddWithValue("@email", newuser.email);
                                adapter2.UpdateCommand.Parameters.AddWithValue("@lname", newuser.lastName);
                                adapter2.UpdateCommand.Parameters.AddWithValue("@email", newuser.email);
                                adapter2.UpdateCommand.Parameters.AddWithValue("@pwd", hash);
                                adapter2.UpdateCommand.Parameters.AddWithValue("@salt", salt);
                                Log.Information("UpdateUserAccount:Executing Update Statement");
                                adapter2.UpdateCommand.ExecuteNonQuery();
                                response.statusCode = 103;
                                response.message = "User Updated Successfully";
                                Log.Information("UpdateUserAccount:User Updated Successfully");
                                return JsonConvert.SerializeObject(response);
                            }
                            else
                            {
                                Log.Information("UpdateUserAccount:User Email does not exist");
                                response.statusCode = 005;
                                response.message = "User Email Address or Password is incorrect.";
                                return JsonConvert.SerializeObject(response);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                response.statusCode = 996;
                response.message = "Update User Error:" + ex.ToString();
                Log.Information("UpdateUserAccount:Update Error: "+ ex.ToString());
                return JsonConvert.SerializeObject(response);
            }
        }

        string HashPasword(string password, out string saltconvert)
        {
            Log.Information("HashPasword:Function Start");
            byte[] salt = RandomNumberGenerator.GetBytes(keySize);
            saltconvert = Convert.ToHexString(salt);
            salt = Encoding.UTF8.GetBytes(saltconvert); 
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        bool VerifyPassword(string password, string hash, string saltconvert)
        {
            Log.Information("VerifyPassword:Function Start");
            byte[] salt = Encoding.UTF8.GetBytes(saltconvert);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
