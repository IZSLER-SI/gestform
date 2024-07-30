<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="Events.aspx.vb" Inherits="GestioneFormazione.FrontOffice.Events" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">

        var searchEventi;
        var searchEventiTop;

        $(function () {

            searchEventi = $("#updSearchEventi");
            searchEventiTop = searchEventi.position().top;

            $(this).scroll(function () {
                var pageTop = $(this).scrollTop();
                if (pageTop > searchEventiTop - 25) {
                    searchEventi.css("position", "fixed");
                    searchEventi.css("top", "25px");
                }
                else {
                    searchEventi.css("position", "");
                    searchEventi.css("top", "");
                }
            });
        });
    </script>
    <div class="twocol_left">
        <asp:UpdatePanel ID="updEventi" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdEventi" runat="server" EnableViewState="false" />        
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="twocol_right">
        <asp:UpdatePanel ID="updSearchEventi" runat="server" EnableViewState="true" UpdateMode="Conditional" ClientIDMode="Static">
            <ContentTemplate>
                <div class="title blue">
                    RICERCA EVENTI
                </div>
                <div class="datagroup top20" style="width:100%">
                    <div class="row" style="height:28px;">
                        <div class="label">
                            Mese/Anno
                        </div>
                        <div class="data right">
                            <asp:DropDownList ID="ddnMeseAnno" runat="server" CssClass="ddn" Width="206px" />
                        </div>
                    </div>
                    <div class="row" style="height:28px;">
                        <div class="label">
                            Profilo
                        </div>
                        <div class="data right">
                            <asp:DropDownList ID="ddnProfilo" runat="server" CssClass="ddn" Width="206px" />
                        </div>
                    </div>
                    <div class="row" style="height:28px;">
                        <div class="label">
                            Sede
                        </div>
                        <div class="data right">
                            <asp:DropDownList ID="ddnSede" runat="server" CssClass="ddn" Width="206px" />
                        </div>
                    </div>
                    <div class="row" style="height:28px;">
                        <div class="label">
                            ECM
                        </div>
                        <div class="data right">
                            <asp:DropDownList ID="ddnEcm" runat="server" CssClass="ddn" Width="206px" />
                        </div>
                    </div>
                    <div class="row" style="height:28px;">
                        <div class="label">
                            Titolo (contiene)
                        </div>
                        <div class="data right">
                            <asp:TextBox ID="txtTitolo" runat="server" CssClass="txt" Width="202px" />
                        </div>
                    </div>
                </div>
                <div style="font-size:14px;padding-top:10px;">
                    <asp:LinkButton runat="server" ID="lnkCerca" CssClass="btnlink btnlink_blue" Font-Bold="true">Cerca</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:LinkButton runat="server" ID="lnkPulisci" CssClass="btnlink btnlink_blue">Pulisci</asp:LinkButton>
                </div>
                <!-- controlli nascosti -->
                <asp:HiddenField runat="server" ID="hidMeseAnno" />
                <asp:HiddenField runat="server" ID="hidProfilo" />
                <asp:HiddenField runat="server" ID="hidSede" />
                <asp:HiddenField runat="server" ID="hidEcm" />
                <asp:HiddenField runat="server" ID="hidTitolo" />
                <asp:HiddenField runat="server" ID="hidSearchActive" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="clear"></div>
</asp:Content>
