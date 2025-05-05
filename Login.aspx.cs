using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ecommerce.User
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the page only executes postback logic if the page is being submitted
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Get the username and password entered by the user
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Mock authentication (replace with actual database or API validation)
            if (username == "admin" && password == "password")
            {
                // Redirect to home page or dashboard after successful login
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                // Show error message for invalid credentials
                lblMessage.Text = "Invalid username or password.";
            }
        }
    }
}