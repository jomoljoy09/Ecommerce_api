using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace MyFirstProject.Models
{
    public class DAL
    {
        public Response register(Users users, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_register", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName",users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            cmd.Parameters.AddWithValue("@Fund", 0);
            cmd.Parameters.AddWithValue("@Type", "Users");
            cmd.Parameters.AddWithValue("@Status", "Active");
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User registered successfully";
            }
            else
            {
                response.StatusCode= 100;
                response.StatusMessage = "User registration failed";
            }
            return response;
        }

        public Response login(Users users, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_login", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Email", users.Email);
            da.SelectCommand.Parameters.AddWithValue("@Password", users.Password);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if(dt.Rows.Count > 0)
            {
                //user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                //user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                //user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                //user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                //user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                response.StatusCode = 200;
                response.StatusMessage = "User is valid";
                response.users = users;
            }
            else
            {
                response.StatusCode= 100;
                response.StatusMessage = "User is invalid";
                response.users = null;
            }
            return response;
        }

        //public Response viewUser(Users users, SqlConnection connection)
        //{
        //    SqlDataAdapter da = new SqlDataAdapter("p_viewUser", connection);
        //    da.SelectCommand.CommandType = CommandType.StoredProcedure;
        //    da.SelectCommand.Parameters.AddWithValue("@ID", users.ID);

        //    DataTable dt = new DataTable();
        //    da.Fill(dt);

        //    Response response = new Response();
        //    Users user = new Users();

        //    if (dt.Rows.Count > 0)
        //    {
        //        user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
        //        user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
        //        user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
        //        user.Email = Convert.ToString(dt.Rows[0]["Email"]);
        //        user.Type = Convert.ToString(dt.Rows[0]["Type"]);
        //        user.Fund = Convert.ToDecimal(dt.Rows[0]["Fund"]);

        //        // Return response with status 200 (user found)
        //        response.StatusCode = 200;
        //        response.StatusMessage = "User exists";
        //        response.users = user; // Populate user object in the response
        //    }
        //    else
        //    {
        //        // If no user found, return 100 status code
        //        response.StatusCode = 100;
        //        response.StatusMessage = "User does not exist";
        //        response.users = users;
        //    }

        //    return response;
        //}

        public Response viewUser(string email, SqlConnection connection)
        {
            // Prepare the command to call the stored procedure
            SqlDataAdapter da = new SqlDataAdapter("p_viewUser", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            // Add the email parameter to the stored procedure
            da.SelectCommand.Parameters.AddWithValue("@Email", email);

            // Create a DataTable to hold the result
            DataTable dt = new DataTable();

            // Fill the DataTable with the result of the stored procedure execution
            da.Fill(dt);

            // Create a response object to store the result
            Response response = new Response();

            if (dt.Rows.Count > 0)
            {
                // If a user is found, populate the user object
                Users user = new Users
                {
                    ID = Convert.ToInt32(dt.Rows[0]["ID"]),
                    FirstName = Convert.ToString(dt.Rows[0]["FirstName"]),
                    LastName = Convert.ToString(dt.Rows[0]["LastName"]),
                    Email = Convert.ToString(dt.Rows[0]["Email"]),
                    Type = Convert.ToString(dt.Rows[0]["Type"]),
                    Fund = Convert.ToDecimal(dt.Rows[0]["Fund"]),
                    Password = Convert.ToString(dt.Rows[0]["Password"])
                };

                // Set the response with status 200 and the user data
                response.StatusCode = 200;
                response.StatusMessage = "User exists";
                response.users = user;
            }
            else
            {
                // If no user is found, set the response status to 100 (User does not exist)
                response.StatusCode = 100;
                response.StatusMessage = "User does not exist";
                response.users = null;
            }

            // Return the response object
            return response;
        }



        public Response updateProfile(Users users, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_updateProfile", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Record updated successfully";
            }
            else
            {
                response.StatusCode= 100;
                response.StatusMessage = "Some error occured. Try after some time";
            }
            return response;
        }

        public Response addToCart(Cart cart, SqlConnection connection)
        {
            Response response=new Response();
            SqlCommand cmd = new SqlCommand("sp_AddToCart",connection);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", cart.UserId);
            cmd.Parameters.AddWithValue("@UnitPrice", cart.UnitPrice);
            cmd.Parameters.AddWithValue("@Discount", cart.Discount);
            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
            cmd.Parameters.AddWithValue("@TotalPrice", cart.TotalPrice);
            cmd.Parameters.AddWithValue("@ItemId", cart.ItemID);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close ();
            if (i > 0) {
                response.StatusCode = 200;
                response.StatusMessage = "Item added successfully";
            }
            else
            {
                response.StatusCode= 100;
                response.StatusMessage = "Item not added";
            }
            return response;
        }

        public Response placeOrder(Users user, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_PlaceOrder", connection);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID",user.ID);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close ();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Order has been placed successfully";
            }
            else
            {
                response.StatusCode= 100;
                response.StatusMessage = "Order could not be placed";
            }
            return response;
        }

        public Response orderList(Users user, SqlConnection connection)
        {
            Response response = new Response();
            List<Orders> listOrder = new List<Orders>();
            SqlDataAdapter da = new SqlDataAdapter("sp_OrderList", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Type", user.Type);
            da.SelectCommand.Parameters.AddWithValue("@ID", user.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Orders orders = new Orders();
                    orders.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    orders.OrderNo = Convert.ToString(dt.Rows[i]["OrderNo"]);
                    orders.OrderTotal = Convert.ToDecimal(dt.Rows[i]["OrderTotal"]);
                    orders.OrderStatus = Convert.ToString(dt.Rows[i]["OrderStatus"]);
                    listOrder.Add(orders);
                }
                if (listOrder.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Order details fetched";
                    response.listOrders = listOrder;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Order not available";
                    response.listOrders = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Order not available";
                response.listOrders = null;
            }
            return response;
        }


        public Response AddUpdateItem(Items item, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_AddUpdateItem", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@title", item.title);
            cmd.Parameters.AddWithValue("@category", item.category);
            cmd.Parameters.AddWithValue("@description", item.description);
            cmd.Parameters.AddWithValue("@price", item.price);
            cmd.Parameters.AddWithValue("@image", item.image);

            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();

            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Item inserted/updated successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Failed to save item. Please try again.";
            }

            return response;
        }
    

    public Response userList(SqlConnection connection)
        {
            Response response = new Response();
            List<Users> listUsers = new List<Users>();
            SqlDataAdapter da = new SqlDataAdapter("sp_UserList", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Users users = new Users();
                    users.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    users.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                    users.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                    users.Password = Convert.ToString(dt.Rows[i]["Password"]);
                    users.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    users.Fund = Convert.ToDecimal(dt.Rows[i]["Fund"]);
                    users.Status = Convert.ToString(dt.Rows[i]["Status"]);
                    users.CreatedOn = Convert.ToDateTime(dt.Rows[i]["CreatedOn"]);
                    listUsers.Add(users);
                }
                if (listUsers.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "User details fetched";
                    response.listUsers = listUsers;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "User not available";
                    response.listUsers = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User not available";
                response.listUsers = null;
            }
            return response;
        }
    }
}
