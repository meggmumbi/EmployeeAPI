using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApiEmpl.Models;

namespace WebApiEmpl.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
            select * from dbo.Employee
            ";
            string sqlConnection = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = new DataTable();
            SqlDataReader myReader;
            
            using(SqlConnection myCon = new SqlConnection(sqlConnection))
            {
                myCon.Open();

                using(SqlCommand cmd = new SqlCommand(query, myCon))
                {
                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"
                   Insert into dbo.Employee(EmployeeName,Department,DateOfJoining,PhotoFileName)
            values
            (
            '" + emp.EmployeeName + @"'
            ,'"+ emp.Department + @"'
            ,'"+emp.DateOfJoining+@"'
            ,'"+emp.PhotoFileName+@"'

            )";
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
        public JsonResult Put(Employee emp)
        {
            string query = @"
            update dbo.Department set 
            EmployeeName = '" + emp.EmployeeName + @"'
            ,Department = '" + emp.Department + @"'
            ,DateOfJoinig = '" + emp.DateOfJoining + @"'

            where EmployeeId = " + emp.EmployeeId + @"
            ";
            //conn string
            string sqlConnection = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlConnection))
            {
                mycon.Open();
                using (SqlCommand cmd = new SqlCommand(query, mycon))
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
            delete from dbo.Employee
            where EmployeeId =" + id + @"
             ";

            string sqlConnection = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = new DataTable();

            SqlDataReader myReader;

            using (SqlConnection mycon = new SqlConnection(sqlConnection))
            {
                mycon.Open();
                using (SqlCommand cmd = new SqlCommand(query, mycon))
                {
                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Delete successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootFileProvider + "/Photo/" + filename;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }

        }
        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            string query = @"
            select DepartmentName  from dbo.Department";
            string sqlConnection = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            DataTable table = new DataTable();

            using(SqlConnection mycon = new SqlConnection(sqlConnection))
            {
                mycon.Open();
                using(SqlCommand cmd = new SqlCommand(query, mycon))
                {
                    myreader = cmd.ExecuteReader();
                    table.Load(myreader);

                    myreader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);

        }

    }
}
