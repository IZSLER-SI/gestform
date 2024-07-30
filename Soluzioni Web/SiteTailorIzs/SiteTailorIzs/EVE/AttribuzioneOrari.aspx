<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="AttribuzioneOrari.aspx.vb" Inherits="Softailor.SiteTailorIzs.AttribuzioneOrari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <link href="<%=Page.ResolveUrl("~/App_Themes/SiteTailorCustom/SiteTailor_CustomGrid.css")%>" rel="stylesheet" />
    <style type="text/css">
        .selezcb input
        {
            margin: 0px;
            padding: 0px;
        }
        .previewimg
        {
            cursor:pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <script src="<%=Page.ResolveUrl("~/Scripts/StlWebUI_CustomGrids.js")%>"></script>
    <script>
        $(function () {
            stl_cus_grd_setupAllGridHeaders();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(cus_EndRequestHandler);
        });

        function stl_cus_grd_setupAllGridHeaders() {
            $("div.stl_cus_upg").each(function (index, element) {
                stl_cus_grd_setupGridHeader(index, element);
            });
        }

        function cus_EndRequestHandler(sender, args) {
            //errore server
            if (args.get_error() != undefined) {
                return;
            }

            //in caso di errore, resetto tutte le variabili e mi fermo
            if (stl_appb_error == true) {
                return;
            }

            //impongo la risistemazione degli header per tutte le griglie che vengono aggiornate
            stl_cus_grd_setupAllGridHeaders();
        }

        function SelAll() {
            $(".selezcb input").prop("checked", true);
        }

        function SelNone() {
            $(".selezcb input").prop("checked", false);
        }
    </script>
    <div class="stl_gen_box" style="position: absolute; width: 230px; top: 0px; left: 0px;">
        <div class="title">
            Ingressi / Uscite da registrare
        </div>
      <div class="content padall" style="height:500px;">
        <asp:UpdatePanel ID="updOrari" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdOrari" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
      </div>
    </div>
    <div class="stl_cus_upg" style="position:absolute;width:800px;height:450px;top:0px;left:250px;">
        <asp:UpdatePanel ID="updIscritti" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdIscritti" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="position:absolute;top:460px;left:250px;width:300px;font-size:11px;">
        <span class="btnlink" onclick="SelAll();">Seleziona tutti</span>
        <span class="btnlink" onclick="SelNone();">Deseleziona tutti</span>
    </div>
    <div style="position:absolute;top:480px;left:250px;width:802px;text-align:right;">
        <asp:UpdatePanel ID="updComandi" runat="server" EnableViewState="false" >
            <ContentTemplate>
                <div style="font-family:Arial;font-size:15px;font-weight:bold;">
                    <asp:LinkButton ID="lnkGo" runat="server" CssClass="btnlink">Registra ingressi/uscite</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="cnfGo" TargetControlID="lnkGo" runat="server" ConfirmText="Confermi l'operazione?" />
                </div>
                <div style="font-weight:bold;padding-top:5px;">
                    <asp:Label ID="lblResult" runat="server" EnableViewState="false"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
