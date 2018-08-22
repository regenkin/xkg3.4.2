<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="AddSpecification.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.AddSpecification" %>
<%@ Register TagPrefix="cc1" TagName="SpecificationView" Src="~/Admin/Goods/ascx/SpecificationView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #newPreview { FILTER: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)}
    </style>  
     <script type="text/javascript" language="javascript">
          
         function gotourl()
         {
             location.href = "ProductTypes.aspx";
         }
       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
          <div class="page-header">
            <h2>添加新的商品类型</h2>
            <small>商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，一个商品类型下可添加多种属性.一种是供客户查看的扩展属性,如图书类型商品的作者，出版社等，一种是供客户可选的规格,如服装类型商品的颜色、尺码。</small>
        </div>
         <div id="mytabl">
            <!-- Nav tabs -->
            <ul class="nav nav-tabs">
                <li><a href="#home">第一步：添加类型名称</a></li>
                <li ><a href="#profile">第二步：添加扩展属性</a></li>
                <li class="active"><a href="#messages">第三步：添加规格</a></li>

            </ul>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">

                     <cc1:SpecificationView runat="server" ID="specificationView" />
               
                   <div class="form-group">
                        <div class="col-xs-offset-4 col-xs-10">
                       
                            <button type="button" value="下一步" onclick="gotourl();" class="btn btn-primary">下一步</button>
         
                        </div>
                    </div>
                </div>

            </div>
        </div>
 
        </form>
</asp:Content>

 

