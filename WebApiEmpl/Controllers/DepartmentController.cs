using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApiEmpl.Models;

namespace WebApiEmpl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        //To access the configuration from appsetting file use dependency injection

        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                   select DepartmentId, DepartmentName from dbo.Department";
            DataTable table = new DataTable();

            //store db connection string
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            //
            SqlDataReader myreader;
            using(SqlConnection mycon=new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    myreader = myCommand.ExecuteReader();
                    table.Load(myreader);

                    myreader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);

                        

        }

        [HttpPost]
        public JsonResult Post(Department dep) 
        {
            string query = @"
                   Insert into dbo.Department values ('"+dep.DepartmentName+@"')";
            DataTable table = new DataTable();

            //store db connection string
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            //
            SqlDataReader myreader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    myreader = myCommand.ExecuteReader();
                    table.Load(myreader);

                    myreader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Succesfully");
        }

        [HttpPut("{id}")]
        public JsonResult Put(Department dep)
        {
            string query = @"
            update dbo.Department set 
            DepartmentName = '"+dep.DepartmentName+@"'
            where DepartmentId = "+dep.DepartmentId+@"
            ";
            //conn string
            string sqlConnection = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using(SqlConnection mycon = new SqlConnection(sqlConnection))
            {
                mycon.Open();
                using(SqlCommand cmd = new SqlCommand(query, mycon))
                {
                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();

                }
            }

            return new JsonResult("Updated successfully");

        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
            delete from dbo.Department
            where DepartmentId ="+ id +@"
             ";

            string sqlConnection = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = new DataTable();

            SqlDataReader myReader;

            using(SqlConnection mycon = new SqlConnection(sqlConnection))
            {
                mycon.Open();
                using(SqlCommand cmd = new SqlCommand(query, mycon))
                {
                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Delete successfully");
        }



       
       

    }
}
