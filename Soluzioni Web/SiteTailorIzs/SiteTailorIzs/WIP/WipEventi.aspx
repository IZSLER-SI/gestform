<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="WipEventi.aspx.vb" Inherits="Softailor.SiteTailorIzs.WipEventi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .wipevento ul
        {
            padding:0px;
            margin:0px 0px 0px 20px;
        }
        .wipevento .expired
        {
            color:#ff0000;
        }
        .wipevento .active
        {
            color:#000000;
        }
        .wipevento .normal
        {
            color:#000000;
        }
        .wipevento .evea
        {
            color:#336699;
            text-decoration:none;
        }
        .wipevento .evea:hover
        {
            color:#336699;
            text-decoration:underline;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
