using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ecommerce.Admin
{
    public partial class SubCategory : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["breadCumbTitle"] = "Manage Sub-Category";
            Session["breadCumbPage"] = "Sub-Category";
            if (!IsPostBack)
            {
                getCategories();
                getSubCategories();
            }
            lblMsg.Visible = false;
        }

        void getCategories()
        {
            con = new SqlConnection(Utils.getConnection());
            cmd = new SqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "GETALL");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
        }

        void getSubCategories()
        {
            con = new SqlConnection(Utils.getConnection());
            cmd = new SqlCommand("SubCategory_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "GETALL");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rSubCategory.DataSource = dt;
            rSubCategory.DataBind();
        }

        protected void btnAddOrUpdate_Click(Object sender, EventArgs e)
        {
            string actionName = string.Empty;
            int subCategoryId = Convert.ToInt32(hfSubCategoryId.Value);
            con = new SqlConnection(Utils.getConnection());
            cmd = new SqlCommand("SubCategory_Crud", con);
            cmd.Parameters.AddWithValue("@Action", subCategoryId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@SubCategoryId", subCategoryId);
            cmd.Parameters.AddWithValue("@SubCategoryName", txtSubCategoryName.Text.Trim());
            cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(ddlCategory.SelectedValue));
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);
            cmd.CommandType = CommandType.StoredProcedure;
            
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                actionName = subCategoryId == 0 ? " inserted" : "Updated";
                lblMsg.Visible = true;
                lblMsg.Text = "Sub-Category" + actionName + " successfully!";
                lblMsg.CssClass = "alert alert-success";
                getSubCategories();
                clear();
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Error- " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
            finally
            {
                con.Close();
            }
        }

        protected void btnClear_Click(Object sender, EventArgs e)
        {
            clear();
        }

        void clear()
        {
            txtSubCategoryName.Text = string.Empty;
            cbIsActive.Checked = false;
            hfSubCategoryId.Value = "0";
            btnAddOrUpdate.Text = "Add";
            ddlCategory.ClearSelection();
        }

        protected void rSubCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            if (e.CommandName == "edit")
            {
                con = new SqlConnection(Utils.getConnection());
                cmd = new SqlCommand("SubCategory_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@SubCategoryId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                txtSubCategoryName.Text = dt.Rows[0]["SubCategoryName"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                ddlCategory.SelectedValue = dt.Rows[0]["CategoryId"].ToString();
                // Fix: Use SubCategoryId instead of CategoryId
                hfSubCategoryId.Value = dt.Rows[0]["SubCategoryId"].ToString();
                btnAddOrUpdate.Text = "Update";
            }
            else if (e.CommandName == "delete")
            {
                con = new SqlConnection(Utils.getConnection());
                cmd = new SqlCommand("SubCategory_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@SubCategoryId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Sub-Category deleted successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getSubCategories();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error- " + ex.Message;
                    lblMsg.CssClass = "alert alert-dander";
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}