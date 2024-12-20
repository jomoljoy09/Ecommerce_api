using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.Models;
using System.Data.SqlClient;

namespace MyFirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //[HttpPost]
        //[Route("AddUpdateItem")]
        //public async Task<IActionResult> AddUpdateItem([FromForm] Items items)
        //{
        //    //Console.WriteLine($"Title: {items.title}, Description: {items.description}, Price: {items.price}, Category: {items.category}, Image: {items.image?.FileName}");

        //    DAL dal = new DAL();
        //    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EItems")))
        //    {
        //        Response response = dal.AddUpdateItem(items, connection);
        //        return Ok(response);
        //    }
        //}


        [HttpPost("AddUpdateItem")]
        public async Task<IActionResult> AddUpdateItem([FromForm] Items item)
        {
            if (item == null || string.IsNullOrEmpty(item.title) || string.IsNullOrEmpty(item.image))
            {
                return BadRequest("Title, Description, Category, and Image are required.");
            }

            try
            {
                // Assuming image is the URL or a file path
                string imagePath = item.image; // If it's a URL or file path

                // Example of how to save the file locally or return a URL
                // Depending on your configuration, you may want to save the file to disk or upload it to a cloud provider

                var uploadFolder = _configuration.GetValue<string>("UploadFolder"); // Folder configured in appsettings.json

                // If you want to handle actual file upload (optional, if using cloud storage)
                if (item.image != null)
                {
                    var filePath = Path.Combine(uploadFolder, item.image);
                    // Save or process the file path
                }

                // Save your data (title, description, etc.)
                // Here, you can save the details (title, description, category) in the database or in memory

                return Ok(new { Message = "Item added successfully", ImageUrl = imagePath });
            }
            catch (Exception ex)
            {
                // Log error and return an appropriate message
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        //public Response AddUpdateItem(Items items)
        //{
        //    DAL dal = new DAL();
        //    SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EItems").ToString());
        //    Response response = new Response();
        //    response = dal.AddUpdateItem(items, connection);
        //    return response;
        //}

        [HttpGet]
        [Route("userList")]

        public Response userList()
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EItems").ToString());
            Response response = new Response();
            response = dal.userList(connection);
            return response;
        }

    }
}
