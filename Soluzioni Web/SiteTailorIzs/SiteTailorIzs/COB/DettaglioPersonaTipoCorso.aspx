<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="DettaglioPersonaTipoCorso.aspx.vb" Inherits="Softailor.SiteTailorIzs.DettaglioPersonaTipoCorso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <link href="DettaglioPersonaTipoCorso.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="tworows_1">
        <asp:Label ID="lblTitolo1" EnableViewState="false" runat="server">Titolo 1 ggg</asp:Label>
    </div>
    <div class="tworows_2" style="font-weight:bold;">
        <asp:Label ID="lblTitolo2" EnableViewState="false" runat="server">Titolo 2 ggg</asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a class="tbbtn" href="javascript:parent.stl_sel_done('');">
            <span class="icon close"></span>
            Chiudi
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="phdOutOfForm" runat="server">
</asp:Content>
