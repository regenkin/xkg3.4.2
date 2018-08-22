<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUeditor.ascx.cs" Inherits="Hidistro.UI.Web.hieditor.ueditor.controls.ucUeditor" %>
<asp:TextBox ID="txtMemo" runat="server" TextMode="MultiLine" ></asp:TextBox>
<%if(IsFirstEdit){ if(ShowType==2){%>
    <script type="text/javascript" charset="utf-8" src="/hieditor/ueditor/ueditorWX.config.js"></script><%}else{ %>
    <script type="text/javascript" charset="utf-8" src="/hieditor/ueditor/ueditor.config.js"></script><%} %>
    <script type="text/javascript" charset="utf-8" src="/hieditor/ueditor/ueditor.all.js"></script>
    <script type="text/javascript" src="/hieditor/ueditor/lang/zh-cn/zh-cn.js"></script><%} %>
<script type="text/javascript">
    <%if(IsFirstEdit){ %>
    var um = UE.getEditor('<%=txtMemo.ClientID%>');<%}else{%>
    var um<%=txtMemo.ClientID%> = UE.getEditor('<%=txtMemo.ClientID%>');
    <%}%>
</script>
