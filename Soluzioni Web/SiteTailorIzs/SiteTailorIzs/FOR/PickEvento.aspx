<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="PickEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.PickEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .evento
        {
            font-size:11px;
            line-height:14px;
            border:1px solid #c0c0c0;
            background-color:#f5f5f5;
            padding:5px;
            margin-bottom:10px;
            margin-right:10px;
            cursor:pointer;
        }
            .evento:hover
            {
                background-color:#c6eee8;
            }
    </style>
    <script>

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">
        Selezione evento inserito precedentemente
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a class="tbbtn" onclick="javascript:parent.stl_sel_done('');">
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
