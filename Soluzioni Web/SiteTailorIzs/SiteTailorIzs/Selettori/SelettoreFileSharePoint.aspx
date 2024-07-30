<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="SelettoreFileSharePoint.aspx.vb" Inherits="Softailor.SiteTailorIzs.SelettoreFileSharePoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .file
        {
            width:700px;
            padding:3px;
            border:1px solid #c0c0c0;
            margin-bottom:5px;
            background-color:#f5f5f5;
            line-height:32px;
            font-family:Calibri;
            font-size:18px;
            cursor:pointer;
            color:#000000;
        }
            .file:hover
            {
                background-color:#336699;
                color:#ffffff;
            }
            .file img
            {
                line-height:32px;
                vertical-align:middle;
                margin-right:5px;
            }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">
        Selezione File
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
     <div class="buttonsection">
        <a class="tbbtn" href="javascript:parent.stl_sel_done('');">
            <span class="icon close"></span>
            Annulla
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
