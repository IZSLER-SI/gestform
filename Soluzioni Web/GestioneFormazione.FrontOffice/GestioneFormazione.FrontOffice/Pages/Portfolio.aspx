<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="Portfolio.aspx.vb" Inherits="GestioneFormazione.FrontOffice.Portfolio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">

        function showMaterialPopup(show) {
            if (show) {
                $("#mat_popup_covering").fadeIn(0, function () {
                    $("#mat_popup").fadeIn(0);
                });
            }
            else {
                $("#mat_popup").fadeOut(250, function () {
                    $("#mat_popup_covering").fadeOut(100);
                });
            }
        }
    </script>
    <div class="onecol">
        <div class="title green bottom20">
            Portfolio formativo
        </div>
        <asp:UpdatePanel ID="updContent" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="mat_popup_covering" style="display:none;"></div>
    <div id="mat_popup" style="display:none;">
        <asp:UpdatePanel ID="updMateriale" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdMateriale" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
