<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="GestioneQuestionarioQualita.aspx.vb" Inherits="Softailor.SiteTailorIzs.GestioneQuestionarioQualita" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <link href="GestioneQuestionarioQualita.css" rel="stylesheet" />
    <script src="GestioneQuestionarioQualita.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">
        <asp:Literal ID="ltrTitolo" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server" EnableViewState="false">
        <ContentTemplate>
            <div class="stl_gen_box" style="position:absolute;top:15px;left:15px;width:840px;">
                <div class="content" style="height:480px">
                    <asp:PlaceHolder ID="phdRisposte" runat="server" EnableViewState="false" />
                </div>
            </div>
            <div style="position:absolute;top:510px;left:457px;width:400px;text-align:right;font-size:13px;font-weight:bold;">
                <asp:LinkButton ID="lnkSalvaChiudi" runat="server" CssClass="btnlink">
                    Salva e chiudi
                </asp:LinkButton>
                <ajaxToolkit:ConfirmButtonExtender ID="cnfSalvaChiudi" runat="server" TargetControlID="lnkSalvaChiudi" ConfirmText="Confermi il salvataggio?" />
                &nbsp;
                <asp:LinkButton ID="lnkChiudi" runat="server" CssClass="btnlink">
                    Chiudi senza salvare
                </asp:LinkButton>
                <ajaxToolkit:ConfirmButtonExtender ID="cnfChiudi" runat="server" TargetControlID="lnkChiudi" ConfirmText="Confermi l'abbandono dei dati inseriti?" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
