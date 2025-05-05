using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Ecommerce.User
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Retrieve form field values
            string name = txtName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();
            string postCode = txtPostCode.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Validate the fields
            string validationMessage = ValidateInputs(name, username, mobile, email, address, postCode, password, confirmPassword);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                lblMessage.Text = validationMessage;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Attempt to register the user
            bool isRegistered = RegisterUser(name, username, mobile, email, address, postCode, password);

            if (isRegistered)
            {
                lblMessage.Text = "Registration successful! Please log in.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                ClearForm();
            }
            else
            {
                lblMessage.Text = "Registration failed. Please try again.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private string ValidateInputs(string name, string username, string mobile, string email, string address, string postalCode, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(name)) return "Name is required.";
            if (string.IsNullOrEmpty(username)) return "Username is required.";
            if (string.IsNullOrEmpty(mobile)) return "Mobile number is required.";
            if (!Regex.IsMatch(mobile, @"^(09|\+639)\d{9}$")) return "Invalid mobile number. It must start with 09 or +639.";
            if (string.IsNullOrEmpty(email)) return "Email is required.";
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return "Invalid email format.";
            if (string.IsNullOrEmpty(address)) return "Address is required.";
            if (string.IsNullOrEmpty(postalCode)) return "Postal code is required.";
            if (!Regex.IsMatch(postalCode, @"^\d{4}$")) return "Postal code must be a 4-digit number.";
            if (string.IsNullOrEmpty(password)) return "Password is required.";
            if (password.Length < 6) return "Password must be at least 6 characters long.";
            if (password != confirmPassword) return "Passwords do not match.";

            return string.Empty; // Validation passed
        }

        private bool RegisterUser(string name, string username, string mobile, string email, string address, string postalCode, string password)
        {
            // Connection string to connect to your database
            string connectionString = "Data Source=ZAI;Initial Catalog=ECommerce;Integrated Security=True;Trust Server Certificate=True";

            // SQL query to insert the user data into the Users table
            string query = @"
                INSERT INTO Users (Name, Username, Mobile, Email, Address, PostCode, Password, ImageUrl, RoleId, CreatedDate)
                VALUES (@Name, @Username, @Mobile, @Email, @Address, @PostCode, @Password, @ImageUrl, @RoleId, @CreatedDate)";

            try
            {
                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the database connection
                    connection.Open();

                    // Create a command object with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Mobile", string.IsNullOrEmpty(mobile) ? (object)DBNull.Value : mobile);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);
                        command.Parameters.AddWithValue("@PostCode", string.IsNullOrEmpty(postalCode) ? (object)DBNull.Value : postalCode);
                        command.Parameters.AddWithValue("@Password", HashPassword(password)); // Hash the password
                        command.Parameters.AddWithValue("@ImageUrl", DBNull.Value); // Default NULL for ImageUrl
                        command.Parameters.AddWithValue("@RoleId", 2); // Default role (e.g., "2" for regular users)
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now); // Current date and time

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Return true if at least one row is affected
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (replace with proper logging in production)
                Console.WriteLine("Error during registration: " + ex.Message);

                // Return false to indicate failure
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Use a simple hashing method (e.g., SHA256 or BCrypt)
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void ClearForm()
        {
            // Clear all form fields
            txtName.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPostCode.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
        }
    }
}