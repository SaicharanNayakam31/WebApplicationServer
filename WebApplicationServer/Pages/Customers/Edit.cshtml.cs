using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text;

namespace WebApplicationServer.Pages.Customers
{
    public class EditModel : PageModel
    {
        public CustomerInfo customerInfo = new CustomerInfo();
        public string errorMessage = "";
        public string successMessage = "";
        
        public void OnGet()
        { 

            string id = Request.Query["id"];
            try
            {
                string connectionString = "Data Source =.; Initial Catalog = mystore; Integrated Security = True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);  
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                customerInfo.id = "" + reader.GetInt32(0);
                                customerInfo.name = reader.GetString(1);
                                customerInfo.email = reader.GetString(2);
                                customerInfo.phone = reader.GetString(3);
                                customerInfo.address = reader.GetString(4);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;  
            }
        }



        public void OnPost() 
        {
            customerInfo.id = Request.Form["id"];    
            customerInfo.name = Request.Form["name"];
            customerInfo.email = Request.Form["email"];
            customerInfo.phone = Request.Form["phone"];
            customerInfo.address = Request.Form["address"];

            if (customerInfo.id.Length ==0 || customerInfo.name.Length == 0 || customerInfo.email.Length == 0 || customerInfo.phone.Length == 0 || customerInfo.address.Length == 0)
            {
                errorMessage = "All Fields Are Required";
                return;
            }

            try
            {
                string connectionString = "Data Source =.; Initial Catalog = mystore; Integrated Security = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients " + 
                                "SET name=@name, email=@email, phone=@phone, address=@address "+
                                " WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        command.Parameters.AddWithValue("@name", customerInfo.name);
                        command.Parameters.AddWithValue("@email", customerInfo.email);
                        command.Parameters.AddWithValue("@phone", customerInfo.phone);
                        command.Parameters.AddWithValue("@address", customerInfo.address);
                        command.Parameters.AddWithValue("@id", customerInfo.id);


                        command.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return; 
            }

           

            Response.Redirect("/Customers/Index");
        }

      
    }
}
