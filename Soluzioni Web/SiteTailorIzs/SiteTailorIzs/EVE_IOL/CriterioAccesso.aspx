<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="CriterioAccesso.aspx.vb" Inherits="Softailor.SiteTailorIzs.CriterioAccesso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script type="text/javascript">
        <asp:Literal ID="ltrRepositioning" runat="server" />    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <link href="CriterioAccesso.css" rel="stylesheet" />
    <script src="CriterioAccesso.js"></script>
    <div class="singlerow">
        <asp:Literal ID="ltrTitle" runat="server" EnableViewState="false" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a ID="lnkClose" class="tbbtn" href="javascript:confirmClose();">
            <span class="icon close"></span>
            <asp:Literal ID="ltrClose" runat="server" EnableViewState="false" />
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server" EnableViewState="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="phdCriterio" runat="server" EnableViewState="false" />
            <div style="position:absolute;top:780px;left:15px;width:1072px;text-align:right;font-family:Arial;font-size:15px;font-weight:bold;">
                <span id="spanSave" class="btnlink">
                    <asp:Label ID="lblSave" runat="server" EnableViewState="false" />
                </span>
                <div style="display:none;">
                    <asp:LinkButton ID="lnkSave" runat="server">-</asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
