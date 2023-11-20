using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplicationServer.Pages.Customers
{
    public class CreateModel : PageModel
    {
        public CustomerInfo customerInfo = new CustomerInfo();
        public string errorMessage = "";
        public string successmessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            customerInfo.name = Request.Form["name"];   
            customerInfo.email = Request.Form["email"];
            customerInfo.phone = Request.Form["phone"];
            customerInfo.address = Request.Form["address"]; 

            if (customerInfo.name.Length==0 || customerInfo.email.Length==0 || customerInfo.phone.Length==0 || customerInfo.address.Length == 0)
            {
                errorMessage = "All Fields Are Required";
                return;
            }

            //save the new Customer in database

            try
            {
                string connectionString = "Data Source=.;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients" + "(name,email,phone,address)VALUES" +
                                "(@name,@email,@phone,@address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        command.Parameters.AddWithValue("@name", customerInfo.name);
                        command.Parameters.AddWithValue("@email", customerInfo.email);
                        command.Parameters.AddWithValue("@phone", customerInfo.phone);
                        command.Parameters.AddWithValue("@address", customerInfo.address);

                        command.ExecuteNonQuery();
                          

                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage=ex.Message;
                return;
            }

            customerInfo.name = "";
            customerInfo.email = "";
            customerInfo.phone = "";
            customerInfo.address = "";
            successmessage = "New Customer Added SuccessFully!";

            Response.Redirect("/Customers/Index");
        }
    }
}
