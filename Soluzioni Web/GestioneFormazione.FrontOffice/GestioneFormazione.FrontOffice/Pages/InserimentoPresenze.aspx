<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="InserimentoPresenze.aspx.vb" Inherits="GestioneFormazione.FrontOffice.InserimentoPresenze" %>
<asp:Content ID="contentContent" ContentPlaceHolderID="cphContent" runat="server">
    <script>
        function cblSel(id, sel) {
            $("#" + id).find("input[type='checkbox']").prop("checked", sel);
        }
        function confirmCancel() {
            if (window.confirm("Confermi l'abbandono dei dati inseriti?")) location.href = "/";
        }
        function confirmSave() {
            return(window.confirm("*** ATTENZIONE ****\nI dati inseriti non potranno più essere modificati.\n\nConfermi il salvataggio?"));
        }
        function gotoHome() {
            location.href = "/";
        }

    </script>
     <div class="onecol">
        <div class="title blue bottom20">
            Registrazione date svolgimento / orari / presenze
        </div>
         <asp:UpdatePanel ID="updContent" runat="server" EnableViewState="true">
             <ContentTemplate>
                 <asp:Panel ID="pnlDati" runat="server" EnableViewState="true" Visible="true">
                     <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
                 </asp:Panel>
                 <asp:Panel ID="pnlDone" runat="server" EnableViewState="true" Visible="false">
                     <asp:PlaceHolder ID="phdDone" runat="server" EnableViewState="false" />
                 </asp:Panel>
             </ContentTemplate>
         </asp:UpdatePanel>
    </div>
</asp:Content>
