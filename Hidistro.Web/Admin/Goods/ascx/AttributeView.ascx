<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeView.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.Goods.ascx.AttributeView" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<style>
    .SKUValue {
        position: relative;
        float: left;
        margin-top: 3px;
        margin-left: 3px;
        margin-right: 3px;
        white-space: nowrap;
        display: block;
    }

        .SKUValue span.span1 {
            margin: 0px;
            padding: 0px;
        }

            .SKUValue span.span1 a {
                color: #000;
                border: 1px #b2b2b2 solid;
                padding: 0px 10px;
                float: left;
                margin: 0px 5px;
                white-space: nowrap;
            }

        .SKUValue span.span2 {
            position: absolute;
            top: 1px;
            right: 7px;
            width: 10px;
            height: 10px;
            overflow: hidden;
            text-indent: 30px;
            z-index: 99;
            overflow: hidden;
        }

            .SKUValue span.span2 a {
                display: block;
                background: url(../images/tOver.gif) #f2f2f2 no-repeat -8px -7px;
                cursor: pointer;
                overflow: hidden;
            }
            a {
  text-decoration: none;
  color: #0b5ba5;
}
             
        .table_title{background:#f2f2f2}
       table td,th{text-align:center}
    
</style>
<div class="content">
    <UI:Grid CssClass="table table-hover mar table-bordered" ID="grdAttribute" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="AttributeId" HeaderStyle-CssClass="table_title" GridLines="None"
        Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="属性名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%">
                <ItemTemplate>
                    <Hi:HtmlDecodeTextBox ID="txtAttributeName" runat="server" Text='<%# Eval("AttributeName") %>'
                        Width="70px"></Hi:HtmlDecodeTextBox>
                    <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>'
                        Visible="false"></asp:Literal>
                    <asp:LinkButton ID="lbtnSave" style="color:#07D;" Text="修改" runat="server" CommandName="saveAttributeName" />
                </ItemTemplate>
            </asp:TemplateField>
            <UI:YesNoImageColumn DataField="IsMultiView" ItemStyle-Width="9%" HeaderText="支持多选" HeaderStyle-CssClass="td_right td_left" />
            <asp:TemplateField HeaderText="属性值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="45%">
                <ItemTemplate>
                    <asp:Repeater ID="rptSKUValue" runat="server" DataSource='<%# Eval("AttributeValues") %>'>
                        <ItemTemplate>
                            <span class="SKUValue"><span class="span1">
                                <asp:HyperLink ID="HyperLink1" runat="server"><%# Eval("ValueStr")%></asp:HyperLink></span>
                                <span class="span2"><a href="javascript:deleteAttributeValue(this,'<%# Eval("ValueId")%>');">删除</a></span> </span>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:TemplateField>
            <UI:SortImageColumn HeaderText="排序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"
                ItemStyle-Width="7%" />
            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                <ItemStyle CssClass="spanD spanN" />
                <ItemTemplate>
                    <span class="submit_tiajia"><a href="javascript:void(0)" style="color:#07D;" onclick="ShowAddSKUValueDiv('<%# Eval("AttributeId") %>','<%# Eval("AttributeName") %>');">添加属性值</a></span> <span class="submit_bianji">
                        <asp:HyperLink ID="lkbViewAttribute" runat="server" style="color:#07D;" Text="编辑" NavigateUrl='<%#Globals.GetAdminAbsolutePath(string.Format("Goods/EditAttributeValues.aspx?TypeId={0}&AttributeId={1}",Eval("TypeId"),Eval("AttributeId")))%>'></asp:HyperLink></span>
                    <span class="submit_shanchu">
                      <%--  <Hi:ImageLinkButton ID="lkbDelete" CssClass="SmallCommonTextButton" runat="server"
                            IsShow="true" CommandName="Delete" Text="删除" />--%>
                      <asp:Button ID="lkbDelete" runat="server"   Class="btnLink" Text="删除" CommandName="Delete"   OnClientClick="return HiConform('<strong>确定要删除选择的属性吗？</strong><p>删除属性不可恢复！</p>',this)" ToolTip="" /> </span>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </UI:Grid>
</div>
<div style="margin-top: 10px; margin-bottom: 10px;">
    <input type="button" name="button" id="button" value="添加扩展属性" class="btn btn-success"
        onclick="ShowAddKzAtribute();" />
</div>

<%--添加扩展属性--%>
<div class="modal fade" id="myModal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">添加扩展属性</h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>属性值名</label>
                        <div class="col-xs-6">
                            <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputPassword3" class="col-xs-2 control-label">是否支持多选</label>
                        <div class="col-xs-2">
                            <asp:CheckBox ID="chkMulti_copy" Text="支持多选" runat="server" onclick="javascript:SetMultSate(this)" Checked="true" />
                        </div>
                        <small class="help-block">(有些属性是可以选择多个属性值的，如“适合人群”，就可能既适合老年人也适合中年人)</small>
                    </div>

                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>属性值</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtValues" runat="server" Width="300" CssClass="form-control"></asp:TextBox>
                        </div> 

                        
                    </div>
                    <small class="help-block" style="margin-left:130px;">扩展属性的值，<%--多个属性值可用“,”号隔开，--%>每个值的字符数最多50个字符</small>
                </div>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                <asp:Button ID="btnCreate" runat="server" Text="添加扩展属性" CssClass="btn btn-primary" />

            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->


<%--添加属性值--%>

<div class="modal fade" id="myaddAttributeValueModal">

    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="attributevalue">添加属性值</h4>
            </div>
            <div class="modal-body">
                <form class="form-horizontal" id="FormAttributeValue">
                    <div class="form-group" style="height: 30px;">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>属性值名</label>
                        <div class="col-xs-5">
                            <asp:TextBox ID="txtValueStr" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                </form>

            </div>
            <div class="modal-footer">
                 
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                <asp:Button ID="btnCreateValue" runat="server" Text="添加属性值" CssClass="btn btn-primary" />

            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>

<div style="display: none">

    <asp:CheckBox ID="chkMulti" Text="支持多选" runat="server" Checked="true" />
    <input runat="server" type="hidden" id="currentAttributeId" />
</div>



<script type="text/javascript">
    $(function () {


    });


    function SetMultSate(multiobj) {
        if (multiobj.checked) {
            $("#ctl00_ContentPlaceHolder1_attributeView_chkMulti").attr("checked", true);
        } else {
            $("#ctl00_ContentPlaceHolder1_attributeView_chkMulti").attr("checked", false);
        }
    }
    //判断规格值
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, "");//删除前后空格
    }
    var formtype = "";
    function ShowAddSKUValueDiv(attributeId, attributename) {
       
        $("#ctl00_ContentPlaceHolder1_attributeView_txtValueStr").val('');
        $('#aspnetForm').formvalidation({

            'ctl00$ContentPlaceHolder1$attributeView$txtValueStr': {
                validators: {
                    notEmpty: {
                        message: '扩展属性的值，每个值的字符数最多50个字符'
                    } 
                }
            }

        });
        formtype = "addvalue";
        $("#ctl00_ContentPlaceHolder1_attributeView_currentAttributeId").val(attributeId);
        $("#attributevalue").text(attributename + "添加属性值");
        $('#myaddAttributeValueModal').modal('toggle').children().css({
            width: '600px',
            height: '100px'
        })
        $("#myaddAttributeValueModal").modal({ show: true });
    }


    //添加扩展属性
    function ShowAddKzAtribute() {
        $('#aspnetForm').formvalidation({
            'ctl00$ContentPlaceHolder1$attributeView$txtName': {
                validators: {
                    notEmpty: {
                        message: '扩展属性的名称，最多15个字符。'
                    },
                    stringLength: {
                        min: 1,
                        max: 15,
                        message: '扩展属性的名称,1-15个字符'
                    }
                }
            } 

        });
        formtype = "add";
        $("#ctl00_ContentPlaceHolder1_attributeView_txtName").val('');
        $("#ctl00_ContentPlaceHolder1_attributeView_txtValues").val('');
        $('#myModal').modal('toggle').children().css({
            width: '800px'
        })
        $("#myModal").modal({ show: true });

    }


   
    function deleteAttributeValue(obj, valueId) {
        $.ajax({
            url: "AddSpecification.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { ValueId: valueId, isCallback: "true" },
            async: false,
            success: function (data) {
                if (data.Status == "true") {
                    location.reload();
                }
                else {
                    ShowMsg("此属性值有商品在使用，删除失败", false);
                }
            }
        });
    }
</script>

