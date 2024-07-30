<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="EditorFiltro.aspx.vb" Inherits="Softailor.SiteTailorIzs.EditorFiltro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <link href="EditorFiltro.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <script src="EditorFiltro.js"></script>
    <script type="text/javascript">
        <asp:Literal ID="ltrLoadParent" runat="server" />    
    </script>
    <div class="singlerow">
        Modifica Filtro
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <asp:UpdatePanel ID="updButtons" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="buttonsection" id="sezioneAttEcm" runat="server">
                <asp:LinkButton ID="lnkSaveClose" runat="server" CssClass="tbbtn">
                    <span class="icon save"></span>
                    Salva e chiudi
                </asp:LinkButton>
            </div>
            <div class="buttonsection">
                <a class="tbbtn" onclick="parent.stl_sel_done('');">
                    <span class="icon close"></span>
                    Annulla
                </a>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updLoadParent" runat="server" EnableViewState="false">
        <ContentTemplate>
            <div style="display:none;">
                <asp:LinkButton ID="lnkLoadParent" runat="server" EnableViewState="false" />
                <asp:TextBox ID="txtParentXml" runat="server" ClientIDMode="Static" EnableViewState="false" TextMode="MultiLine" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="flt">
        <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="true" />
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
