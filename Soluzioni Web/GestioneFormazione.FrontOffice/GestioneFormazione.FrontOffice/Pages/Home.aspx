<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="Home.aspx.vb" Inherits="GestioneFormazione.FrontOffice.Home" %>
<asp:Content ID="contentContent" ContentPlaceHolderID="cphContent" runat="server">
    <asp:UpdatePanel ID="updReminders" runat="server" EnableViewState="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="phdReminders" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="twocol_left">
    <asp:UpdatePanel ID="updEventi" runat="server" EnableViewState="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="phdEventi" runat="server" EnableViewState="false" />  
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <div class="twocol_right">
        <asp:PlaceHolder ID="phdNews" runat="server" EnableViewState="false" />
    </div>
    <div class="clear"></div>

</asp:Content>
