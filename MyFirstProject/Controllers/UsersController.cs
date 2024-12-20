using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstProject.Models;
using System.Data.SqlClient;

namespace MyFirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration) 
        { 
           _configuration = configuration;
        }

        [HttpPost]
        [Route("registration")]
        public Response register(Users users)
        {
            Response response = new Response();
            DAL dal = new DAL();
            //SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EItems").ToString());
            //response = dal.register(users, connection);
            //return response;
            var connectionString = _configuration.GetConnectionString("EItems");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'EItems' is missing or invalid.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                response = dal.register(users, connection);
            }

            return response;
        }

        [HttpPost]
        [Route("login")]
        public Response login(Users users)
        {
            Response response = new Response();
            DAL dal = new DAL();
            //SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EItems").ToString());
            //response = dal.login(users, connection);
            //return response;
            var connectionString = _configuration.GetConnectionString("EItems");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'EItems' is missing or invalid.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                response = dal.login(users, connection);
            }

            return response;
        }

        //[HttpPost]
        //[Route("viewUser")]
        //public Response viewUser(Users users)
        //{
        //    Response response = new Response();
        //    DAL dal = new DAL();
        //    //SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EItems").ToString());
        //    //response = dal.viewUser(users,connection);
        //    //return response;
        //    var connectionString = _configuration.GetConnectionString("EItems");

        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        throw new InvalidOperationException("The connection string 'EItems' is missing or invalid.");
        //    }

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        response = dal.viewUser(users, connection);
        //    }

        //    return response;
        //}

        [HttpGet]  // Use GET request instead of POST to fetch user details based on ID
        [Route("viewUser/{email}")]  // Accept the ID in the URL path
        public Response viewUser(string email)  // Directly take the user email as a parameter
        {
            Response response = new Response();
            DAL dal = new DAL();

            // Get the connection string from the configuration
            var connectionString = _configuration.GetConnectionString("EItems");

            // Validate the connection string
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'EItems' is missing or invalid.");
            }

            // Use the connection to call the DAL method
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection here
                    response = dal.viewUser(email, connection);  // Pass the email directly to DAL
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;  // Internal Server Error
                    response.StatusMessage = $"An error occurred: {ex.Message}";
                }
                finally
                {
                    connection.Close(); // Ensure the connection is closed
                }
            }

            return response;  // Return the response containing user data or error
        }



        [HttpPost]
        [Route("updateProfile")]
        public Response updateProfile(Users users)
        {
            Response response = new Response();
            DAL dal = new DAL();
            //SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EItems").ToString());
            //response = dal.updateProfile(users, connection);
            //return response;
            var connectionString = _configuration.GetConnectionString("EItems");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'EItems' is missing or invalid.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                response = dal.updateProfile(users, connection);
            }

            return response;

        }


    }
}
