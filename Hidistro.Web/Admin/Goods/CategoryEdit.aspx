<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="CategoryEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.CategoryEdit" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_dropCategories").bind("change", function () {
                Callback($(this).val());
            });
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtCategoryName': {
                    validators: {
                        notEmpty: {
                            message: '分类名称不能为空'
                        },
                        stringLength: {
                            min: 1,
                            max: 60,
                            message: '分类名称不能为空，在1至60个字符之间'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtSKUPrefix': {
                    validators: {
                        regexp: {
                            regexp: /^(?!_)(?!-)[a-zA-Z0-9_-]+$/,
                            message: '前缀长度限制在5个字符以内，前缀只能是字母数字开头，可包含-和_'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtthird': {
                    validators: {
                        //notEmpty: {
                        //    message: '上二级佣金不能为空'
                        //},
                        regexp: {
                            regexp: /^(0|(0+(\.[0-9]{1,2}))|[1-9]([0-9]?)(\.\d{1,2})?)$/,
                            message: '数据类型错误，只能输入实数型数值'
                        }

                    }
                },
                'ctl00$ContentPlaceHolder1$txtsecond': {
                    validators: {
                        //notEmpty: {
                        //    message: '上一级佣金不能为空'
                        //},
                        regexp: {
                            regexp: /^(0|(0+(\.[0-9]{1,2}))|[1-9]([0-9]?)(\.\d{1,2})?)$/,
                            message: '数据类型错误，只能输入实数型数值'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtfirst': {
                    validators: {
                        //notEmpty: {
                        //    message: '成交店铺佣金不能为空'
                        //},
                        regexp: {
                            regexp: /^(0|(0+(\.[0-9]{1,2}))|[1-9]([0-9]?)(\.\d{1,2})?)$/,
                            message: '数据类型错误，只能输入实数型数值'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtPageDesc': {
                    validators: {
                        stringLength: {
                            min: 0,
                            max: 100,
                            message: '告诉搜索引擎此分类浏览页面的主要内容，长度限制在100个字符以内'
                        }
                    }
                }
            })
        });
        function Callback(value) {
            //var liURL = document.getElementById("ctl00_ContentPlaceHolder1_liURL");
            //var txtRewriteName = document.getElementById("ctl00_ContentPlaceHolder1_txtRewriteName");
            var txtSKUPrefix = document.getElementById("ctl00_ContentPlaceHolder1_txtSKUPrefix");
            if (value.length > 0) {
                //liURL.style.display = "none";
                //txtRewriteName.value = "";
                $.ajax({
                    url: "CategoryEdit.aspx",
                    type: 'post', dataType: 'json', timeout: 10000,
                    data: {
                        isCallback: "true",
                        parentCategoryId: value
                    },
                    async: false,
                    success: function (resultData) {
                        txtSKUPrefix.value = resultData.SKUPrefix;

                        var f = resultData.f;
                        var s = resultData.s;
                        var t = resultData.t;
                        if (f == "") {
                            f = "0";
                        }
                        if (s == "") {
                            s = "0";
                        }
                        if (t == "") {
                            t = "0";
                        }
                        $(".exitshopinfo").hide();

                        $("#<%=txtfirst.ClientID%>").val(f).blur();//.attr({ "disabled": "disabled" });
                        $("#<%=txtsecond.ClientID%>").val(s).blur();//.attr({ "disabled": "disabled" });
                        $("#<%=txtthird.ClientID%>").val(t).blur();//.attr({ "disabled": "disabled" });
                    }
                });
            }
            else {
                //liURL.style.display = "";
                txtSKUPrefix.value = "";
                $("#<%=txtfirst.ClientID%>").val("").removeAttr("disabled");
                $("#<%=txtsecond.ClientID%>").val("").removeAttr("disabled");
                $("#<%=txtthird.ClientID%>").val("").removeAttr("disabled");
                $(".exitshopinfo").show();
            }
        }
        function ShowNotes(index) {
            document.getElementById("notes1").style.display = "none";
            document.getElementById("notes2").style.display = "none";
            document.getElementById("notes3").style.display = "none";
            var notesId = "notes" + index;
            document.getElementById(notesId).style.display = "block";

            document.getElementById("tip1").className = "";
            document.getElementById("tip2").className = "";
            document.getElementById("tip3").className = "";
            var tipId = "tip" + index;
            document.getElementById(tipId).className = "off"
        }
</script>        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
				<div class="page-header">
					<h2><%=operatorName %>商品分类</h2>
				</div>
    <form id="aspnetForm" runat="server" class="form-horizontal">

        <div class="form-group">
            <label class="col-xs-2 control-label"><em>*</em>分类名称</label>
            <div class="col-xs-6">
                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control inputw200" MaxLength="60" />
            </div>
        </div>
                    <div class="form-group hide">
                        <label class="col-xs-2 control-label">分类图标</label>
                        <div class="col-xs-6">
                            <Hi:UpImg runat="server" ID="uploader1" IsNeedThumbnail="false" UploadType="topic"  />
                        </div>
                    </div>
                    <div class="form-group" style="display:<%=categoryid>0?"none":"block"%>">
                        <label class="col-xs-2 control-label">选择上级分类</label>
                        <div class="col-xs-6">
                           <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" CssClass="form-control inputw200" autocomplete="off" IsTopCategory="true"/>
                        </div>
                    </div>
                    <div class="form-group" style="display:none">
                        <label class="col-xs-2 control-label">商品类型</label>
                        <div class="col-xs-6">
                           <Hi:ProductTypeDownList runat="server"  ID="dropProductTypes" CssClass="form-control inputw200" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-xs-2 control-label">商家编码前缀</label>
                        <div class="col-xs-6">
                            <asp:TextBox ID="txtSKUPrefix" runat="server" CssClass="form-control inputw200" MaxLength="5" />
                            <small class="help-block">填写以后，发布商品为您自动生成商家编码的时候可以加上此前缀，只能是字母、数字开头、可包含-(减号)、_(下划线)，长度5个字符以内</small>
                        </div>
                    </div>





        <div class="exitshopinfo resize bg" style="display:<%=(categoryid>0||dropCategories.SelectedValue>0)?"none":"block"%>">
                        <div class="form-horizontal">
                            <h3 class="resize">分销佣金设置</h3>
                            <div class="form-group">
                                <label for="input6" class="col-xs-2 control-label"><em><%--*--%></em>上二级佣金比例：</label>
                                <div class="col-xs-4">
                                    <asp:TextBox ID="txtthird" runat="server" CssClass="form-control" MaxLength="5"/>&nbsp;&nbsp;%
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="input7" class="col-xs-2 control-label"><em><%--*--%></em>上一级佣金比例：</label>
                                <div class="col-xs-4">
                                    <asp:TextBox ID="txtsecond" runat="server" CssClass="form-control" MaxLength="5"/>&nbsp;&nbsp;%
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="input8" class="col-xs-2 control-label"><em><%--*--%></em>成交店铺佣金比例：</label>
                                <div class="col-xs-4">
                                    <asp:TextBox ID="txtfirst" runat="server" CssClass="form-control" MaxLength="5"/>&nbsp;&nbsp;%
                                </div>
                            </div>
                        </div>
       </div>


                    <div class="form-group"  id="liURL"  runat="server" style="display:none;">
                        <label class="col-xs-2 control-label">URL重写名称</label>
                        <div class="col-xs-6">
                           <asp:TextBox ID="txtRewriteName" runat="server" CssClass="form-control" MaxLength="50"/>
                        </div>
                    </div>
                    <div class="form-group" style="display:none;">
                        <label class="col-xs-2 control-label">搜索标题</label>
                        <div class="col-xs-6">
                           <asp:TextBox ID="txtPageKeyTitle" runat="server"  CssClass="form-control" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="display:none;">
                        <label class="col-xs-2 control-label">搜索关键字</label>
                        <div class="col-xs-6">
                           <asp:TextBox ID="txtPageKeyWords" runat="server" CssClass="form-control" MaxLength="100" />
                        </div>
                    </div>
                    <div class="form-group" style="display:none;">
                        <label class="col-xs-2 control-label">搜索描述</label>
                        <div class="col-xs-6">
                           <asp:TextBox ID="txtPageDesc" runat="server" CssClass="form-control" MaxLength="100" />
                        </div>
                    </div>
              
                    <div class="form-group">
                        <div class="col-xs-10 col-xs-offset-2">
                            <asp:Button ID="btnSaveCategory" runat="server" Text="保存"  CssClass="btn btn-success float inputw100" />
                <asp:Button ID="btnSaveAddCategory" runat="server" Text="保存并继续添加" CssClass="btn btn-success" />
                          <input type="button" value="返回" onclick="location.href='<%=reurl%>'" class="btn btn-primary inputw100" style="display:<%=categoryid>0?"inline-block":"none"%>" />
                        </div>
                    </div>
    </form>
</asp:Content>
