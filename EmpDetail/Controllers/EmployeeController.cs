
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Empdetailswebapi.Models;

namespace Empdetailswebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration config;
        private string connectionString;
        List<Employee> emp = new List<Employee>();
        public EmployeeController(IConfiguration config)
        {
            this.config = config;
        }


        [HttpGet]
        public IActionResult Get()
        {
            connectionString = config.GetConnectionString("constr");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "select * from Employee";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        //emp.Id = Convert.ToInt32(reader[0]);
                        //emp.Name = Convert.ToString(reader[1]);
                        //emp.Location= Convert.ToString(reader[2]);
                        //emp.Salary= Convert.ToInt32(reader[3]);
                        emp.Add(new Employee()
                        {
                            Id = Convert.ToInt32(reader[0]),
                            Name = Convert.ToString(reader[1]),
                            Location = Convert.ToString(reader[2]),
                            Salary = Convert.ToInt32(reader[3])
                        });

                    }
                }
                return Ok(emp);
            }
            catch (SqlException e)
            {
                return BadRequest("Unable to open sql connection");
            }
            finally
            {
                connection.Close();
            }

        }

        [HttpPost]
        public IActionResult Post([FromBody] Employee employee)
        {
            //int id = emp.Id;
            //string name = emp.Name;
            //string location = emp.Location;
            //int salary = emp.Salary;

            //var serialize = JsonConvert.SerializeObject(employee);
            //JObject empobject = JObject.Parse(serialize);
            connectionString = config.GetConnectionString("constr");
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "insert into Employee values(@p1,@p2,@p3,@p4)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@p1", employee.Id);
            command.Parameters.AddWithValue("@p2", employee.Name);
            command.Parameters.AddWithValue("@p3", employee.Location);
            command.Parameters.AddWithValue("@p4", employee.Salary);


            //command.Parameters.AddWithValue("@p1", empobject["Id"].ToString());
            //command.Parameters.AddWithValue("@p1", empobject["Name"].ToString());
            //command.Parameters.AddWithValue("@p1", empobject["Location"].ToString());
            //command.Parameters.AddWithValue("@p1", empobject["Salary"].ToString());

            //var parameters = new IDataParameter[]
            //    {
            //        new SqlParameter("@p1", empobject["Id"].ToString()),
            //        new SqlParameter("@p2", empobject["Name"].ToString()),
            //        new SqlParameter("@p3",empobject["Location"].ToString()),
            //         new SqlParameter("@p4",empobject["Salary"].ToString())
            //   };
            try
            {
                conn.Open();
                //foreach (IDataParameter p in parameters)
                //{
                //    command.Parameters.Add(p);
                //}


                int rows = command.ExecuteNonQuery();
                if (rows == 1)
                    return Ok(employee);
                return BadRequest("Adding failed");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
