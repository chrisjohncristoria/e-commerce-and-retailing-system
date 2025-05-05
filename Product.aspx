<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="Ecommerce.Admin.Product" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
    /* for dissapearing alert message */
    window.onload = function () {
        var seconds = 5;
        setTimeout(function () {
            document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
        }, seconds * 1000);
    }
</script>
<script>
    function ImagePreview(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var controlName = input.id.substr(input.id.indexOf("_") + 1);
                if (controlName == 'fuFirstImage') {
                    $('#<%=imageProduct1.ClientID%>').show();
                    $('#<%=imageProduct1.ClientID%>').prop('src', e.target.result).width(200).height(200);
                } else if (controlName == 'fuSecondImage') {
                    $('#<%=imageProduct2.ClientID%>').show();
                    $('#<%=imageProduct2.ClientID%>').prop('src', e.target.result).width(200).height(200);
                } else if (controlName == 'fuThirdImage') {
                    $('#<%=imageProduct3.ClientID%>').show();
                    $('#<%=imageProduct3.ClientID%>').prop('src', e.target.result).width(200).height(200);
                } else {
                    $('#<%=imageProduct4.ClientID%>').show();
                    $('#<%=imageProduct4.ClientID%>').prop('src', e.target.result).width(200).height(200);
                }
            };
            reader.readAsDataURL(input.files[0]);
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="mb-4">
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
</div>

    <div class="row">
     <div class="col-sm-12 col-md-12">
       <div class="card">
        <div class="card-body">
            <h4 class="card-title">Product</h4>
            <hr />

            <div class="form-body">

                <div class="row">
                    <div class="col-md-6">
                        <label>Product Name</label>
                        <div class="form-group">
                           <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="Enter Product Name"></asp:TextBox>
                           <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductName"  
                             ErrorMessage="Product Name is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Category</label>
                        <div class="form-group">
                           <asp:DropdownList ID="ddlCategory" runat="server" CssClass="form-control" AppendDataBountItems="true" AutoPostBack="true"
                               OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                              <asp:ListItem Value="0">Select Category</asp:ListItem>
                           </asp:DropdownList>
                           <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlCategory" InitialValue="0" 
                             ErrorMessage="Category is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>SubCategory</label>
                        <div class="form-group">
                           <asp:DropdownList ID="ddlSubCategory" runat="server" CssClass="form-control" AppendDataBountItems="true">
                           </asp:DropdownList>
                           <asp:RequiredFieldValidator ID="rfvSubCategory" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlSubCategory" 
                             ErrorMessage="SubCategory is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row">
                  <div class="col-md-4">
                    <label>Price</label>
                    <div class="form-group">
                       <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Enter Product Price"></asp:TextBox>
                       <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ForeColor="Red" Font-Size="Small" 
                         Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPrice"  
                         ErrorMessage="Price is required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revPrice" runat="server" ControlToValidate="txtPrice"
                            ValidationExpression="\d+(?:.\d{1,2})?" ErrorMessage="Product Price is invalid" ForeColor="Red"
                            Display="Dynamic" SetFocusOnError="true" Font-Size="Smaller"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Color</label>
                    <div class="form-group">
                       <asp:ListBox ID="lboxColor" runat="server" CssClass="form-control" SelectionMode="Multiple"
                           ToolTip="Use CTRL key to select multiple items">
                           <asp:ListItem Value="1">Blue</asp:ListItem>
                           <asp:ListItem Value="2">Red</asp:ListItem>
                           <asp:ListItem Value="3">Pink</asp:ListItem>
                           <asp:ListItem Value="4">Purple</asp:ListItem>
                           <asp:ListItem Value="5">Brown</asp:ListItem>
                           <asp:ListItem Value="6">Gray</asp:ListItem>
                           <asp:ListItem Value="7">Green</asp:ListItem>
                           <asp:ListItem Value="8">Yellow</asp:ListItem>
                           <asp:ListItem Value="9">White</asp:ListItem>
                           <asp:ListItem Value="10">Black</asp:ListItem>
                       </asp:ListBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Size</label>
                    <div class="form-group">
                       <asp:ListBox ID="lboxSize" runat="server" CssClass="form-control" SelectionMode="Multiple"
                           ToolTip="Use CTRL key to select multiple items">
                           <asp:ListItem Value="1">XS</asp:ListItem>
                           <asp:ListItem Value="2">SM</asp:ListItem>
                           <asp:ListItem Value="3">M</asp:ListItem>
                           <asp:ListItem Value="4">L</asp:ListItem>
                           <asp:ListItem Value="5">XL</asp:ListItem>
                           <asp:ListItem Value="6">XXL</asp:ListItem>
                       </asp:ListBox>
                    </div>
                </div>

            </div>

                <div class="row">
                   <div class="col-md-6">
                        <label>Quantity</label>
                        <div class="form-group">
                           <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Enter Product Quantity"
                               TextMode="Number"></asp:TextBox>
                           <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQuantity"  
                             ErrorMessage="Product Quantity is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <label>Company Name</label>
                        <div class="form-group">
                           <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" placeholder="Enter Product's Company Name">
                           </asp:TextBox>
                           <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCompanyName"  
                             ErrorMessage="Company Name is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-12">
                        <label>Short Description</label>
                        <div class="form-group">
                           <asp:TextBox ID="txtShortDescription" runat="server" CssClass="form-control" placeholder="Enter Short Description"
                                TextMode="MultiLine">
                           </asp:TextBox>
                           <asp:RequiredFieldValidator ID="rfvShortDescription" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtShortDescription"  
                             ErrorMessage="Short Description is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <label>Long Description</label>
                        <div class="form-group">
                           <asp:TextBox ID="txtLongDescription" runat="server" CssClass="form-control" placeholder="Enter Long Description"
                               TextMode="MultiLine">
                           </asp:TextBox>
                           <asp:RequiredFieldValidator ID="rfvLongDescription" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLongDescription"  
                             ErrorMessage="Long Description is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <label>Addtional Description</label>
                        <div class="form-group">
                           <asp:TextBox ID="txtAdditionalDescription" runat="server" CssClass="form-control" placeholder="Enter Additional Description"
                               TextMode="MultiLine">
                           </asp:TextBox>
                           <asp:RequiredFieldValidator ID="rfvAdditionalDescription" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAdditionalDescription"  
                             ErrorMessage="Additional Description is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <%--<div class="row">
                    <div class="col-md-12">
                        <label>Tags(Search Keyword)</label>
                        <div class="form-group">
                           <asp:TextBox ID="txtTags" runat="server" CssClass="form-control" placeholder="Enter Tags">
                           </asp:TextBox>
                           <asp:RequiredFieldValidator ID="rfvTags" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTags"  
                             ErrorMessage="Product Tags is required"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>--%>  

                <div class="row">
                    <div class="col-md-6">
                        <label>Product Image 1</label>
                        <div class="form-group">
                           <asp:FileUpload ID="fuFirstImage" runat="server" CssClass="form-control" ToolTip=".jpg, .png, .jpeg image only"
                                onchange =" ImagePreview(this);"/>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <label>Product Image 2</label>
                        <div class="form-group">
                           <asp:FileUpload ID="fuSecondImage" runat="server" CssClass="form-control" ToolTip=".jpg, .png, .jpeg image only"
                                onchange =" ImagePreview(this);"/>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <label>Product Image 3</label>
                        <div class="form-group">
                           <asp:FileUpload ID="fuThirdImage" runat="server" CssClass="form-control" ToolTip=".jpg, .png, .jpeg image only"
                                onchange =" ImagePreview(this);"/>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <label>Product Image 4</label>
                        <div class="form-group">
                           <asp:FileUpload ID="fuFourthImage" runat="server" CssClass="form-control" ToolTip=".jpg, .png, .jpeg image only"
                                onchange =" ImagePreview(this);"/>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <label>Default Image</label>
                        <div class="form-group">
                           <asp:RadioButtonList ID="rblDefaultImage" runat="server" RepeatDirection="Horizontal">
                               <asp:ListItem Value="1">&nbsp; First &nbsp;</asp:ListItem>
                               <asp:ListItem Value="2">&nbsp; Second &nbsp;</asp:ListItem>
                               <asp:ListItem Value="3">&nbsp; Third &nbsp;</asp:ListItem>
                               <asp:ListItem Value="4">&nbsp; Fourth &nbsp;</asp:ListItem>
                           </asp:RadioButtonList>
                           <asp:RequiredFieldValidator ID="rfvDefaultImage" runat="server" ForeColor="Red" Font-Size="Small" 
                             Display="Dynamic" SetFocusOnError="true" ControlToValidate="rblDefaultImage"  
                             ErrorMessage="Default Image is required"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hfDefaultImagePos"  runat="server" Value="0"/>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Customized</label>
                        <div class="form-group">
                            <asp:CheckBox ID="cbIsCustomized" runat="server" Text="&nbsp; IsCustomized" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>IsActive</label>
                        <div class="form-group">
                            <asp:CheckBox ID="cbIsActive" runat="server" Text="&nbsp; IsActive" />
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-12 align-content-sm-between pl-3">
                        <span> 
                            <asp:Image ID="imageProduct1" runat="server" CssClass="img-thumbnail" AlternateText="" style="display:none;" />
                        </span>
                        <span> 
                            <asp:Image ID="imageProduct2" runat="server" CssClass="img-thumbnail" AlternateText="" style="display:none;" />
                        </span>
                        <span> 
                            <asp:Image ID="imageProduct3" runat="server" CssClass="img-thumbnail" AlternateText="" style="display:none;" />
                        </span>
                        <span> 
                            <asp:Image ID="imageProduct4" runat="server" CssClass="img-thumbnail" AlternateText="" style="display:none;" />
                        </span>
                    </div>
                </div>

            </div>

            <div class="form-action pb-4">
                <div class="text-left">
                    <asp:Button ID="btnAddOrUpdate" runat="server" CssClass="btn btn-info" Text="Add" OnClick="btnAddOrUpdate_Click" />
                    <asp:Button ID="btnClear" runat="server" CssClass="btn btn-dark" Text="Reset" CausesValidation="false" 
                       OnClick="btnClear_Click" />
                </div>
            </div>

            

        </div>
    </div>
</div>

    </div>
    
</asp:Content>
