<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="DelegatiInserimentoPresenzeFO.aspx.vb" Inherits="Softailor.SiteTailorIzs.DelegatiInserimentoPresenzeFO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script type="text/javascript">
        function undoCreation() {
            if (window.confirm("Confermi l'abbandono dei dati inseriti?")) {
                location.href = "../EVE/HomeEvento.aspx";
            }
        }
        function clearPartecipante(idx) {
            $("#noniscrittoid_" + idx).val("");
            clickPostBack();
        }
        function selectPartecipante(idx)
        {
            $("#partidx").val(idx);
            var url = "../Selettori/SelettorePersonaGForm.aspx?diponly=0";
            stl_sel_display(url, selectPartecipante_CallBack);
        }
        function selectPartecipante_CallBack(id_persona)
        {
            $("#noniscrittoid_" + $("#partidx").val()).val(id_persona);
            clickPostBack();
        }
    </script>
    <style type="text/css">
        .btnicon
        {
            vertical-align: top;
            margin-top: 3px;
            cursor: pointer;
            margin-left: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <script src="DelegatiInserimentoPresenzeFO.js"></script>
    <asp:UpdatePanel ID="updControls" runat="server" EnableViewState="False">
        <ContentTemplate>
            <div style="display:none">
                <input id="partidx" name="partidx" />
                <asp:LinkButton ID="lnkPostBack" runat="server">PostBack</asp:LinkButton>
            </div>
            <script>
                <asp:Literal ID="ltrScripts" runat="server" />
            </script>
            <div class="stl_dfo" style="width: 850px;">
                <div class="title">
                    Attivazione caricamento fogli firme da portale pubblico
                </div>
                <asp:PlaceHolder ID="phdControls" runat="server" EnableViewState="false" />
                <div class="endCommands">
                    <asp:LinkButton ID="lnkSalva" runat="server" CssClass="btnlink" Font-Bold="true">Salva</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="cnfSalva" runat="server" TargetControlID="lnkSalva" ConfirmText="Confermi l'operazione?" />
                    &nbsp;
                <span class="btnlink" onclick="undoCreation();">Annulla</span>
                    &nbsp;
                <asp:Label ID="lblGlobalError" runat="server" EnableViewState="false" Font-Bold="true" ForeColor="Red" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
