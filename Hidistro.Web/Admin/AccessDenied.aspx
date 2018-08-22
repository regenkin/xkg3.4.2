<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/SimplePage.master" CodeBehind="AccessDenied.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AccessDenied" %>
<%@ Import Namespace="Hidistro.Core"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
      /*.content,.footer{width:980px; margin:auto;}*/
      .c_right{border:1px solid #d7e9fc;height:700px;margin:5px 10px;overflow:hidden;}
      .menu_title{width:190px;font-size:18px;line-height:30px;}
      .C_list{display:none;}
      .C_list ul li {background: url(images/C_list_bg.gif) no-repeat;margin-left:12px;vertical-align:middle;width:162px;text-indent: 20px;line-height: 28px; border-bottom: #ccc 1px solid
}
      .sideitem{width:184px; cursor:pointer; text-indent:30px;line-height:28px;height:28px;}
      .sideitem{background:url(images/sideitem.gif) no-repeat}
      .sideitem-curr{width:184px; cursor:pointer; text-indent:30px;line-height:28px;height:28px;}
      .sideitem-curr{background:url(images/sideitem-curr.gif) no-repeat}
      h4{border:1px solid #cccccc;background-color: #f0f7fe; padding-left:8px;font-weight:700;FONT-FAMILY: "宋体","Arial Narrow"}
      .footer{text-align:center;line-height:25px;}
      .middle{width:770px; margin: 150px 5px auto 5px;font-size:14px;text-align:center;}
      </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="mainhtml">	  
	        <div class="c_right">
	            <h4>后台管理系统</h4>
                    <div class="middle"><img src="images/comeBack.gif" />　　<asp:Literal runat="server" ID="litMessage" /></div>
	        </div>
    </div>
</asp:Content>