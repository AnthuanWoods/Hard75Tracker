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
using Serilog;

namespace Hard75API.Controllers
{
    [ApiController]
    [Route("task")]
    public class TaskController
    {
        public readonly IConfiguration _configuration;
        public TaskController(IConfiguration configuration)
        {
            _configuration = configuration;
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/mylog.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }

        [HttpGet]
        [Route("GetTasks")]
        public string GetTaskList(string userID)
        {
            Log.Information("Get Task List");
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                Log.Information("GetTaskList: Opening DB Connection");
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    Log.Information("GetTaskList: Selecting all active tasks");
                    using (SqlCommand sqlcmd2 = new SqlCommand("Select * from Tasks where userID = @userID and taskActive = 1", con))
                    {
                        sqlcmd2.Parameters.AddWithValue("@userID", userID);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = sqlcmd2;
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Log.Information("Found Tasks");
                            List<Hard75Shared.Task> tasks = new List<Hard75Shared.Task>();
                            foreach (DataRow record in dt.Rows)
                            {
                                Hard75Shared.Task task = new Hard75Shared.Task();
                                task.taskName = record[0].ToString();
                                tasks.Add(task);
                                Log.Information(task.taskName);
                            }
                            response.statusCode = 108;
                            response.message = JsonConvert.SerializeObject(tasks);
                            
                            return JsonConvert.SerializeObject(response);
                        }
                        else
                        {
                            Log.Information("No Task Found");
                            response.statusCode = 109;
                            response.message = "{}";
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.ToString());
                response.statusCode = 992;
                response.message = "Select Task Error:" + ex.ToString();
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("CreateTask")]
        public string CreateTask([FromBody] Hard75Shared.Task task)
        {
            Log.Information("CreateTask: Started");
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                Log.Information("CreateTask: Opening Connection"); 
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    Log.Information("CreateTask: Running Query");
                    using (SqlCommand sqlcmd2 = new SqlCommand("Insert into Tasks(taskName, taskDescription,taskParentID, taskCategoryID,taskActive,userID) Values (@taskName, @taskDescription, @taskParent, @taskCategory,1,@userID)", con))
                    {
                        sqlcmd2.Parameters.AddWithValue("@taskName", task.taskName);
                        sqlcmd2.Parameters.AddWithValue("@taskDescription", task.taskDescription);
                        sqlcmd2.Parameters.AddWithValue("@taskParent", task.parentTaskID);
                        sqlcmd2.Parameters.AddWithValue("@taskCategory", task.categoryID);
                        sqlcmd2.Parameters.AddWithValue("@userID", task.userID);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = sqlcmd2;
                        adapter.InsertCommand.ExecuteNonQuery();
                        response.statusCode = 110;
                        response.message = "Task Created Successfully";
                        Log.Information("CreateTask: Task Created");
                        return JsonConvert.SerializeObject(response);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information("CreateTask: Error " + ex.ToString());
                response.statusCode = 991;
                response.message = "Create Task Error:" + ex.ToString();
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("UpdateTask")]
        public string UpdateTask([FromBody] Hard75Shared.Task task)
        {
            Log.Information("UpdateTask: Started"); 
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                Log.Information("UpdateTask: Opening Connection");
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnectionString").ToString()))
                {
                    con.Open();
                    Log.Information("UpdateTask: Running Query");
                    using (SqlCommand sqlcmd2 = new SqlCommand("Update Tasks set taskName = @taskName, taskDescription = @taskDescription, taskParentID = @taskParent, taskCategoryID = @taskCategory, active = @tactive where taskID = @tid and userID = @userid", con))
                    {
                        sqlcmd2.Parameters.AddWithValue("@taskName", task.taskName);
                        sqlcmd2.Parameters.AddWithValue("@taskDescription", task.taskDescription);
                        sqlcmd2.Parameters.AddWithValue("@taskParent", task.parentTaskID);
                        sqlcmd2.Parameters.AddWithValue("@taskCategory", task.categoryID);
                        sqlcmd2.Parameters.AddWithValue("@userID", task.userID);
                        sqlcmd2.Parameters.AddWithValue("@tactive", task.active);
                        sqlcmd2.Parameters.AddWithValue("@tid", task.taskID);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.UpdateCommand = sqlcmd2;
                        adapter.UpdateCommand.ExecuteNonQuery();
                        response.statusCode = 111;
                        response.message = "Task Updated Successfully";
                        Log.Information("UpdateTask: Task Updated");
                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information("UpdateTask: Error " + ex.ToString());
                response.statusCode = 990;
                response.message = "Update Task Error:" + ex.ToString();
                return JsonConvert.SerializeObject(response);
            }
        }
    }
}
