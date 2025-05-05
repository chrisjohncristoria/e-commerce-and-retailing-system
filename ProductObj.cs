using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ecommerce
{
    public class ProductObj
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string AdditionalDescription { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string CompanyName { get; set; }

        //public string Tags { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public bool IsCustomized { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<ProductImageObj> ProductImages { get; set; } = new List<ProductImageObj>();

        public int DefaultImagePosition { get; set; }
    }

    public class ProductImageObj 
    {
        public int ImageId { get; set; }
        
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }

        public bool DefaultImage { get; set; }
    }

    public class ProductDAL
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader sdr;
        SqlDataAdapter sda;
        DataTable dt;
        SqlTransaction transaction = null;

        public int AddUpdateProduct(ProductObj productBO)
        {
            int result = 0;
            int productId = 0;
            string type = "insert";
            using(con = new SqlConnection(Utils.getConnection()))
            {
                try
                {
                    var productImages = productBO.ProductImages;
                    #region insert product
                    con.Open();
                    transaction = con.BeginTransaction();
                    productId = productBO.ProductId;

                    cmd = new SqlCommand("Product_Crud", con, transaction);
                    cmd.Parameters.AddWithValue("@Action", productId == 0 ? "INSERT" : "UPDATE");
                    cmd.Parameters.AddWithValue("@ProductName", productBO.ProductName);
                    cmd.Parameters.AddWithValue("@ShortDescription", productBO.ShortDescription);
                    cmd.Parameters.AddWithValue("@LongDescription", productBO.LongDescription);
                    cmd.Parameters.AddWithValue("@AdditionalDescription", productBO.AdditionalDescription);
                    cmd.Parameters.AddWithValue("@Price", productBO.Price);
                    cmd.Parameters.AddWithValue("@Quantity", productBO.Quantity);
                    cmd.Parameters.AddWithValue("@Size", productBO.Size);
                    cmd.Parameters.AddWithValue("@Color", productBO.Color);
                    cmd.Parameters.AddWithValue("@CompanyName", productBO.CompanyName);
                    cmd.Parameters.AddWithValue("@CategoryId", productBO.CategoryId);
                    cmd.Parameters.AddWithValue("@SubCategoryId", productBO.SubCategoryId);
                    cmd.Parameters.AddWithValue("@IsCustomized", productBO.IsCustomized);
                    cmd.Parameters.AddWithValue("@IsActive", productBO.IsActive);
                    if(productId > 0)
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productBO.ProductId);
                        type = "update";
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    if (productId == 0)
                    {
                        cmd = new SqlCommand("Produuct_Crud", con, transaction);
                        cmd.Parameters.AddWithValue("@Action", "RECENT_PRODUCT");
                        cmd.CommandType = CommandType.StoredProcedure;
                        sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            productId = (int)sdr["ProductId"];
                        }
                        sdr.Close(); 
                    }
                    #endregion

                    #region insert product images
                    if(productId > 0)
                    {
                        if (type == "insert")
                        {
                            foreach (var image in productImages)
                            {
                                cmd = new SqlCommand("Produuct_Crud", con, transaction);
                                cmd.Parameters.AddWithValue("@Action", "INSERT_PROD_IMG");
                                cmd.Parameters.AddWithValue("@ImageUrl", image.ImageUrl);
                                cmd.Parameters.AddWithValue("@ProductId", productId);
                                cmd.Parameters.AddWithValue("@DefaultImage", image.DefaultImage);
                                cmd.CommandType = CommandType.StoredProcedure;
                                sdr = cmd.ExecuteReader();
                                result = 1;
                            } 
                        }
                        else //update images
                        {
                            bool isTrue = false;
                            if(productImages.Count != 0)
                            {
                                cmd = new SqlCommand("Produuct_Crud", con, transaction);
                                cmd.Parameters.AddWithValue("@Action", "DELETE_PROD_IMG");
                                cmd.Parameters.AddWithValue("@ProductId", productId);
                                cmd.CommandType = CommandType.StoredProcedure;
                                sdr = cmd.ExecuteReader();
                                isTrue = true;
                            }
                            else
                            {
                                int defaultImagePos = productBO.DefaultImagePosition;
                                if (defaultImagePos > 0)
                                {
                                    cmd = new SqlCommand("Produuct_Crud", con, transaction);
                                    cmd.Parameters.AddWithValue("@Action", "UPDATE_IMG_POS");
                                    cmd.Parameters.AddWithValue("@ProductId", productId);
                                    cmd.Parameters.AddWithValue("@DefaultImagePos", defaultImagePos);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    sdr = cmd.ExecuteReader();
                                    result = 1;
                                }
                                else
                                {
                                    result = 1;
                                }
                            }
                            if (isTrue)
                            {
                                foreach (var image in productImages)
                                {
                                    cmd = new SqlCommand("Produuct_Crud", con, transaction);
                                    cmd.Parameters.AddWithValue("@Action", "INSERT_PROD_IMG");
                                    cmd.Parameters.AddWithValue("@ImageUrl", image.ImageUrl);
                                    cmd.Parameters.AddWithValue("@ProductId", productId);
                                    cmd.Parameters.AddWithValue("@DefaultImage", image.DefaultImage);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    sdr = cmd.ExecuteReader();
                                    result = 1;
                                }
                            }   
                        }
                    }
                    #endregion

                    transaction.Commit();
                }
                catch (Exception )
                {
                    try
                    {
                        transaction.Rollback();
                        result = 0;
                    }
                    catch (Exception )
                    {
                        throw;
                    }
                }
            }
            return result;
        }

        public DataTable ProductByIdWithImages(int productId)
        {
            try
            {
                DataTable dt = ProductById(productId);
                dt.Columns.Add("Image2");
                dt.Columns.Add("Image2");
                dt.Columns.Add("Image2");
                dt.Columns.Add("DefaultImage");
                DataRow dr = dt.NewRow();
                string images = dt.Rows[0]["Image1"].ToString();
                string[] imgArr = images.Split(';');
                string imag;
                int rb = 0;
                foreach (string img in imgArr)
                {
                    imag = img.Substring(img.IndexOf(": ") + 1);
                    if (imag.Trim() == "1")
                    {
                        break;
                    }
                    else
                    {
                        rb++;
                    }
                }

                foreach (DataRow dataRow in dt.Rows)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        dataRow["image" + (i + 1)] = imgArr[i].Trim();
                    }
                    dataRow["DefaultImage"] = rb;
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable ProductById(int pId)
        {
            try
            {
                using(con = new SqlConnection(Utils.getConnection()))
                {
                    con.Open();
                    cmd = new SqlCommand("Product_Crud", con);
                    cmd.Parameters.AddWithValue("@Action", "GETBYID");
                    cmd.Parameters.AddWithValue("@ProductId", pId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}