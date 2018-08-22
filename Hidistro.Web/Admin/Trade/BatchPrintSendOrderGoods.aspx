<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="BatchPrintSendOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.BatchPrintSendOrderGoods" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /**
 *    打印相关
*/@media print
        {
            .notprint
            {
                display: none;
            }
            .PageNext
            {
                page-break-after: always;
            }
        }
        @media screen
        {
            .notprint
            {
                display: inline;
                cursor: pointer;
            }
        }
        html, legend
        {
            color: #404040;
            background: #fff;
        }
        body, div, dl, dt, dd, ul, ol, li, h1, h2, h3, h4, h5, h6, pre, code, form, fieldset, legend, input, button, textarea, p, blockquote, th, td
        {
            margin: 0;
            padding: 0;
        }
        li
        {
            list-style: none;
        }
        body
        {
            font: 12px/1.5 Tahoma,Helvetica,Arial, '\5b8b\4f53' ,sans-serif;
            text-align: center;
        }
        table
        {
            font-size: inherit;
            font: 100%;
        }
        .clear-both
        {
            clear: both;
        }
        #main
        {
            text-align: left;
            width: 63em;
            margin: 0 auto;
        }
        table
        {
            width: 100%;
            border-collapse: collapse;
            border-spacing: 0;
            border: 1px solid #858585;
        }
        h3
        {
            margin: 0;
            font-size: 12px;
        }
        .print .info
        {
            position: relative;
        }
        .print .info h3
        {
            font-size: 1.2em;
        }
        .print .info .prime-info
        {
            float: left;
        }
        .print ul.sub-info
        {
            overflow: hidden;
        }
        .clear
        {
            clear: both;
        }
        .print ul.sub-info li
        {
            float: left;
            padding-right: 3em;
        }
        button
        {
            width: 10em;
            height: 2em;
            text-align: center;
        }
        th
        {
            color: #000;
            text-align: left;
            padding: .4em .4em .4em 1.1em;
            border-bottom: 1px solid #858585;
            font-weight: 1;
        }
        td
        {
            border-bottom: 1px solid #858585;
            padding: .5em .8em;
            word-break: break-all;
            text-align:left;
            vertical-align: middle;
        }
        .odd td
        {
            background-color: #F2F2F2;
        }
        .price
        {
            
        }
        .price li
        {
            float: left;
            margin: .5em 0;
            min-height: 1%;
            padding-right: 2em;
        }
        .price li span
        {
            float: left;
        }
        .order .col-0
        {
            width: 6em;
        }
        .order .col-1
        {
            width: 13em;
        }
        .order .col-2
        {
            width: 12em;
        }
        .order .col-3
        {
            width: 4em;
        }
        .order .col-4
        {
            width: 4em;
        }
        .order .col-5
        {
            width: 8em;
        }
        .order .col-6
        {
            width: 8em;
        }
    </style>

    <script type="text/javascript">
        function clicks() {
            window.print();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center;width:700px;margin:0 auto;" id="divContent" runat="server" class="">
    </div>
    <div class="notprint navbar-fixed-bottom">
        <input type="button" value="打印" class="btn btn-primary inputw100" id="printBtn" onclick="clicks()" />&nbsp;&nbsp;&nbsp;
        <input type="button" value="关闭" class="btn btn-default inputw100" onclick="window.opener = '';window.close();" /></div>
</asp:Content>
