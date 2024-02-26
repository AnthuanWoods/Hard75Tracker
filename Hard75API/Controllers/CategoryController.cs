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
using System.Data.SqlTypes;

namespace Hard75API.Controllers
{
    [ApiController]
    [Route("category")]
    public class CategoryController
    {
        public readonly IConfiguration _configuration;

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetCategories")]
        public string GetCategoryList(string userID)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    using (SqlCommand sqlcmd2 = new SqlCommand("Select * from Category where userID = @userID", con))
                    {
                        sqlcmd2.Parameters.AddWithValue("@userID", userID);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = sqlcmd2;
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Hard75Shared.Category[] categories = [];
                            foreach (DataRow record in dt.Rows)
                            {
                                Hard75Shared.Category category = new Hard75Shared.Category();
                                category.categoryName = record[0].ToString();
                                categories.Append(category);
                            }
                            response.statusCode = 104;
                            response.message = JsonConvert.SerializeObject(categories);
                            return JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            response.statusCode = 105;
                            response.message = "{}";
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 995;
                response.message = "Select Category Error:" + ex.ToString();
                return JsonConvert.SerializeObject(response);
            }
        }
        [HttpPost]
        [Route("CreateCategory")]
        public string CreateCategory([FromBody] Hard75Shared.Category cat)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    using (SqlCommand sqlcmd2 = new SqlCommand("Insert into Category(categoryName,active,userID) Values (@catName,1,@userID)", con))
                    {
                        sqlcmd2.Parameters.AddWithValue("@catName", cat.categoryName); 
                        sqlcmd2.Parameters.AddWithValue("@userID", cat.userID); 
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = sqlcmd2;
                        adapter.InsertCommand.ExecuteNonQuery();
                        response.statusCode = 106;
                        response.message = "Category Created Successfully";
                        return JsonConvert.SerializeObject(response);
                    }
                }
            } catch (Exception ex)
            {
                response.statusCode = 994;
                response.message = "Create Category Error:" + ex.ToString();
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("UpdateCategory")]
        public string UpdateCategory([FromBody] Hard75Shared.Category cat)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    using (SqlCommand sqlcmd2 = new SqlCommand("Update Category set categoryName = @catname, active = @catactive where categoryID = @catid and userID = @userid", con))
                    {
                        sqlcmd2.Parameters.AddWithValue("@catname", cat.categoryName);
                        sqlcmd2.Parameters.AddWithValue("@userid", cat.userID);
                        sqlcmd2.Parameters.AddWithValue("@catactive", cat.active);
                        sqlcmd2.Parameters.AddWithValue("@catid", cat.categoryID);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.UpdateCommand = sqlcmd2;
                        adapter.UpdateCommand.ExecuteNonQuery();
                        response.statusCode = 107;
                        response.message = "Category Updated Successfully";
                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 993;
                response.message = "Update Category Error:" + ex.ToString();
                return JsonConvert.SerializeObject(response);
            }
        }
    }
}
