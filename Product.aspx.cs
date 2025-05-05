using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Ecommerce.Admin
{
    public partial class Product : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt, dt1;
        string[] imagePath;
        ProductObj productObj;
        ProductDAL productDAL;
        List<ProductImageObj> productImages = new List<ProductImageObj>();
        int defaultImgAfterEdit = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["breadCumbTitle"] = "Product";
            Session["breadCumbPage"] = "Product";
            if (!IsPostBack)
            {
                getCategories();
                if (Request.QueryString["id"] != null)
                {
                    GetProductDetails();
                }
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

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            getSubCategories(Convert.ToInt32(ddlCategory.SelectedValue));
        }

        void getSubCategories(int categoryId)
        {
            con = new SqlConnection(Utils.getConnection());
            cmd = new SqlCommand("SubCategory_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SUBCATEGORYBYID");
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt1 = new DataTable();
            sda.Fill(dt1);
            ddlSubCategory.Items.Clear();
            ddlSubCategory.DataSource = dt1;
            ddlSubCategory.DataTextField = "SubCategoryName";
            ddlSubCategory.DataValueField = "SubCategoryId";
            ddlSubCategory.DataBind();
            ddlSubCategory.Items.Insert(0, "Select SubCategory");
        }

        void GetProductDetails()
        {
            if (Request.QueryString["id"] != null)
            {
                int productId = Convert.ToInt32(Request.QueryString["id"]);
                productDAL = new ProductDAL();
                dt = productDAL.ProductByIdWithImages(productId);
                if (dt.Rows.Count > 0)
                {
                    txtProductName.Text = dt.Rows[0]["ProductName"].ToString();
                    txtPrice.Text = dt.Rows[0]["Price"].ToString();
                    txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                    txtShortDescription.Text = dt.Rows[0]["ShortDescription"].ToString();
                    txtLongDescription.Text = dt.Rows[0]["LongDescription"].ToString();
                    txtAdditionalDescription.Text = dt.Rows[0]["AdditionalDescription"].ToString();
                    string[] color = dt.Rows[0]["Color"].ToString().Split('\u002C');
                    string[] size = dt.Rows[0]["Size"].ToString().Split('\u002C');
                    for (int i = 0; i < color.Length -1; i++)
                    {
                        lboxColor.Items.FindByText(color[i]).Selected = true;
                    }
                    for (int i = 0; i < size.Length - 1; i++)
                    {
                        lboxSize.Items.FindByText(size[i]).Selected = true;
                    }
                    txtCompanyName.Text = dt.Rows[0]["ProductName"].ToString();
                    ddlCategory.SelectedValue = dt.Rows[0]["CategoryId"].ToString();
                    getSubCategories(Convert.ToInt32(dt.Rows[0]["CategoryId"]));
                    ddlSubCategory.SelectedValue = dt.Rows[0]["SubCategoryId"].ToString();
                    cbIsCustomized.Checked = Convert.ToBoolean(dt.Rows[0]["IsCustomized"]);
                    cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                    rblDefaultImage.SelectedIndex = Convert.ToInt32(dt.Rows[0]["DefaultImage"]);
                    hfDefaultImagePos.Value = (Convert.ToInt32(dt.Rows[0]["DefaultImage"]) + 1).ToString();
                    imageProduct1.ImageUrl = "../" + dt.Rows[0]["Image1"].ToString().Substring(0, dt.Rows[0]["Image1"].ToString().IndexOf(":"));
                    imageProduct2.ImageUrl = "../" + dt.Rows[0]["Image2"].ToString().Substring(0, dt.Rows[0]["Image2"].ToString().IndexOf(":"));
                    imageProduct3.ImageUrl = "../" + dt.Rows[0]["Image3"].ToString().Substring(0, dt.Rows[0]["Image3"].ToString().IndexOf(":"));
                    imageProduct4.ImageUrl = "../" + dt.Rows[0]["Image4"].ToString().Substring(0, dt.Rows[0]["Image4"].ToString().IndexOf(":"));
                    imageProduct1.Width = 200;
                    imageProduct2.Width = 200;
                    imageProduct3.Width = 200;
                    imageProduct4.Width = 200;
                    imageProduct1.Style.Remove("display");
                    imageProduct2.Style.Remove("display");
                    imageProduct3.Style.Remove("display");
                    imageProduct4.Style.Remove("display");
                    btnAddOrUpdate.Text = "Update";
                }
            }
        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedColor = string.Empty;
                string selectedSize = string.Empty;
                bool isValid = false;
                bool isValidToExecute = false;
                List<string> list = new List<string>();
                bool isImageSaved = false;
                if (Request.QueryString["id"] == null)
                {
                    if (fuFirstImage.HasFile && fuSecondImage.HasFile && fuThirdImage.HasFile && fuFourthImage.HasFile)
                    {
                        list.Add(fuFirstImage.FileName);
                        list.Add(fuSecondImage.FileName);
                        list.Add(fuThirdImage.FileName);
                        list.Add(fuFourthImage.FileName);
                        string[] fu = list.ToArray();

                        #region validate images
                        for (int i = 0; i <= fu.Length - 1; i++)
                        {
                            if (Utils.isValidExtension(fu[i]))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid &= false;
                                break;
                            }
                        }
                        #endregion

                        #region After image validation proceeding to add product
                        if (isValid)
                        {
                            imagePath = Utils.getImagesPath(fu);
                            for (int i = 0; i <= imagePath.Length - 1; i++)
                            {
                                for (int j = i; j <= rblDefaultImage.Items.Count - 1;)
                                {
                                    productImages.Add
                                    (
                                        new ProductImageObj()
                                        {
                                            ImageUrl = imagePath[i],
                                            DefaultImage = Convert.ToBoolean(rblDefaultImage.Items[j].Selected)
                                        }
                                    );
                                    break;
                                }

                                #region saving all images
                                if (i == 0)
                                {
                                    fuFirstImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + imagePath[i].Replace("Images/Product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 1)
                                {
                                    fuSecondImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + imagePath[i].Replace("Images/Product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 2)
                                {
                                    fuThirdImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + imagePath[i].Replace("Images/Product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 3)
                                {
                                    fuFourthImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + imagePath[i].Replace("Images/Product/", ""));
                                    isImageSaved = true;
                                }
                                #endregion
                            }

                            #region saving new product
                            if (isImageSaved)
                            {
                                selectedColor = Utils.getItemWithCommaSeparater(lboxColor);
                                selectedSize = Utils.getItemWithCommaSeparater(lboxSize);
                                productDAL = new ProductDAL();
                                productObj = new ProductObj()
                                {
                                    ProductId = Request.QueryString["id"] == null ? 0 : Convert.ToInt32(Request.QueryString["id"]),
                                    ProductName = txtProductName.Text.Trim(),
                                    ShortDescription = txtShortDescription.Text.Trim(),
                                    LongDescription = txtLongDescription.Text.Trim(),
                                    AdditionalDescription = txtAdditionalDescription.Text.Trim(),
                                    Price = Convert.ToDecimal(txtPrice.Text.Trim()),
                                    Quantity = Convert.ToInt32(txtQuantity.Text.Trim()),
                                    Size = selectedSize,
                                    Color = selectedColor,
                                    CompanyName = txtCompanyName.Text.Trim(),
                                    CategoryId = Convert.ToInt32(ddlCategory.SelectedValue),
                                    SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue),
                                    IsCustomized = cbIsCustomized.Checked,
                                    IsActive = cbIsActive.Checked,
                                    ProductImages = productImages
                                };
                                int r = productDAL.AddUpdateProduct(productObj);
                                if (r > 0)
                                {
                                    DisplayMessage("Product saved successful.", "success");
                                    Response.AddHeader("REFRESH", "2;URL=ProductList.aspx");
                                }
                                else
                                {
                                    DeleteFile(imagePath);
                                    DisplayMessage("Cannot save record at this moment", "warning");
                                }
                            }
                            else
                            {
                                DeleteFile(imagePath);
                            }
                            #endregion
                        }
                        else
                        {
                            DisplayMessage("Please select .jpg, .jpeg, .png file for image!", "warning");
                        }
                        #endregion
                    }
                    else
                    {
                        DisplayMessage("Please select all product images", "warning");
                    }
                }
                else
                {
                    if (fuFirstImage.HasFile && fuSecondImage.HasFile && fuThirdImage.HasFile && fuFourthImage.HasFile)
                    {
                        list.Add(fuFirstImage.FileName);
                        list.Add(fuSecondImage.FileName);
                        list.Add(fuThirdImage.FileName);
                        list.Add(fuFourthImage.FileName);
                        string[] fu = list.ToArray();

                        #region validate images
                        for (int i = 0; i <= fu.Length - 1; i++)
                        {
                            if (Utils.isValidExtension(fu[i]))
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid &= false;
                                break;
                            }
                        }
                        #endregion

                        #region After image validation proceeding to add product
                        if (isValid)
                        {
                            imagePath = Utils.getImagesPath(fu);
                            for (int i = 0; i <= imagePath.Length - 1; i++)
                            {
                                for (int j = i; j <= rblDefaultImage.Items.Count - 1;)
                                {
                                    productImages.Add
                                    (
                                        new ProductImageObj()
                                        {
                                            ImageUrl = imagePath[i],
                                            DefaultImage = Convert.ToBoolean(rblDefaultImage.Items[j].Selected)
                                        }
                                    );
                                    break;
                                }

                                #region saving all images
                                if (i == 0)
                                {
                                    fuFirstImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + imagePath[i].Replace("Images/Product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 1)
                                {
                                    fuSecondImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + imagePath[i].Replace("Images/Product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 2)
                                {
                                    fuThirdImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + imagePath[i].Replace("Images/Product/", ""));
                                    isImageSaved = true;
                                }
                                else if (i == 3)
                                {
                                    fuFourthImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + imagePath[i].Replace("Images/Product/", ""));
                                    isImageSaved = true;
                                }
                                #endregion
                            }

                            if (isImageSaved)
                            {
                                isValidToExecute = true;
                            }
                            else
                            {
                                DeleteFile(imagePath);
                            }  
                        }
                        else
                        {
                            DisplayMessage("Please select .jpg, .jpeg, .png file for image!", "warning");
                        }
                        #endregion
                    }
                    else if (fuFirstImage.HasFile || fuSecondImage.HasFile || fuThirdImage.HasFile || fuFourthImage.HasFile)
                    {
                        DisplayMessage("Please add all 4 images if want to update images", "warning");
                    }
                    else
                    {
                        //update product without image
                        if(Convert.ToUInt32(hfDefaultImagePos.Value) != Convert.ToUInt32(rblDefaultImage.SelectedValue))
                        {
                            defaultImgAfterEdit = Convert.ToInt32(rblDefaultImage.SelectedValue);
                        }
                        isValidToExecute = true;
                    }

                    #region updating product
                    if (isValidToExecute)
                    {
                        selectedColor = Utils.getItemWithCommaSeparater(lboxColor);
                        selectedSize = Utils.getItemWithCommaSeparater(lboxSize);
                        productDAL = new ProductDAL();
                        productObj = new ProductObj()
                        {
                            ProductId = Convert.ToInt32(Request.QueryString["id"]),
                            ProductName = txtProductName.Text.Trim(),
                            ShortDescription = txtShortDescription.Text.Trim(),
                            LongDescription = txtLongDescription.Text.Trim(),
                            AdditionalDescription = txtAdditionalDescription.Text.Trim(),
                            Price = Convert.ToDecimal(txtPrice.Text.Trim()),
                            Quantity = Convert.ToInt32(txtQuantity.Text.Trim()),
                            Size = selectedSize,
                            Color = selectedColor,
                            CompanyName = txtCompanyName.Text.Trim(),
                            CategoryId = Convert.ToInt32(ddlCategory.SelectedValue),
                            SubCategoryId = Convert.ToInt32(ddlSubCategory.SelectedValue),
                            IsCustomized = cbIsCustomized.Checked,
                            IsActive = cbIsActive.Checked,
                            ProductImages = productImages,
                            DefaultImagePosition = defaultImgAfterEdit
                        };
                        int r = productDAL.AddUpdateProduct(productObj);
                        if (r > 0)
                        {
                            DisplayMessage("Product updateed successful.", "success");
                            Response.AddHeader("REFRESH", "2;URL=ProductList.aspx");
                        }
                        else
                        {
                            DeleteFile(imagePath);
                            DisplayMessage("Cannot update at this moment", "warning");
                        }
                    }
                    else
                    {
                        DisplayMessage("Something went wrong.", "danger");
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void DeleteFile(string[] filePath)
        {
            for (int i = 0; i < filePath.Length -1; i++)
            {
                if (File.Exists(Server.MapPath("~/" + filePath[i])))
                {
                    File.Delete(Server.MapPath("~/" + filePath[i]));
                }
            }
        }

        void DisplayMessage(string message, string cssClass)
        {
            lblMsg.Visible = true;
            lblMsg.Text = message;
            lblMsg.CssClass = "alert alert-" + cssClass;
        }

        private void Clear()
        {
            txtProductName.Text = string.Empty;
            txtShortDescription.Text = string.Empty;
            txtLongDescription.Text = string.Empty;
            txtAdditionalDescription.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            lboxColor.ClearSelection();
            lboxSize.ClearSelection();
            ddlCategory.ClearSelection();
            ddlSubCategory.ClearSelection();
            rblDefaultImage.ClearSelection();
            cbIsCustomized.Checked = false;
            cbIsActive.Checked = false;
            hfDefaultImagePos.Value = "0";
        }

    }
}