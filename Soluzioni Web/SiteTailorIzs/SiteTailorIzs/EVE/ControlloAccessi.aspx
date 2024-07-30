<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ControlloAccessi.aspx.vb" Inherits="Softailor.SiteTailorIzs.ControlloAccessi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <link href="ControlloAccessiStyles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <script type="text/javascript">
        var sndout;
        var sndin;
        var sndno;
        $(function () {
            sndin = new Audio("Sounds/LetturaIN.mp3");
            sndout = new Audio("Sounds/LetturaOUT.mp3");
            sndno = new Audio("Sounds/LetturaNO.mp3");
        });
        function playin() {
            $get("txtBarcode").focus();
            sndin.play();
        }
        function playout() {
            $get("txtBarcode").focus();
            sndout.play();
        }
        function playno() {
            $get("txtBarcode").focus();
            sndno.play();
        }
        function editIscritto_callback(codice) {
            $get("txtBarcode").focus();
        }
        function showPersone(quali) {
            var url = "StatoPresenze.aspx?show=" + quali;
            wopen(url, 'showPersone', 700, 600, 1, 0, 1, 1, 1);
        }
    </script>
    <div class="stl_dfo titBarcode">
        <div class="title">
            Lettura tessera sanitaria o badge
        </div>
    </div>
    <asp:UpdatePanel ID="updBarcode" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlBarcode" runat="server" CssClass="pnlBarcode">
                <span class="lblBarcode">Barcode</span>
                <asp:TextBox runat="server" ID="txtBarcode" CssClass="txtBarcode" AutoComplete="off" MaxLength="40" ClientIDMode="Static" />
                <asp:Button ID="btnBarcode" runat="server" Text="Vai" CssClass="btnlink btnBarcode" style="padding:3px 19px 4px 17px;margin:0px;" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="stl_dfo titCognomeNome">
        <div class="title">
            Registrazione manuale ingresso/uscita
        </div>
    </div>
    <asp:UpdatePanel ID="updCognomeNome" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlCognomeNome" runat="server" CssClass="pnlCognomeNome">
                <span class="lblCognomeNome">Nominativo</span>
                <asp:DropDownList ID="ddnCognomeNome" runat="server" EnableViewState="false" CssClass="ddnCognomeNome" AutoPostBack="true" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="stl_dfo titLetture">
        <div class="title">
            Ultimi movimenti/errori registrati
        </div>
    </div>
    <asp:UpdatePanel ID="updLetture" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlLetture" runat="server" CssClass="pnlLetture">
                <asp:HiddenField ID="l1id_accesso" runat="server" />
                <asp:HiddenField ID="l1id_iscritto" runat="server" />
                <asp:HiddenField ID="l2id_accesso" runat="server" />
                <asp:HiddenField ID="l2id_iscritto" runat="server" />
                <asp:HiddenField ID="l3id_accesso" runat="server" />
                <asp:HiddenField ID="l3id_iscritto" runat="server" />
                <asp:HiddenField ID="l4id_accesso" runat="server" />
                <asp:HiddenField ID="l4id_iscritto" runat="server" />
                <asp:HiddenField ID="l5id_accesso" runat="server" />
                <asp:HiddenField ID="l5id_iscritto" runat="server" />
                <div class="lettura">
                    <asp:Label ID="l1name" runat="server" CssClass="llet nome inactive"/>
                    <asp:Label ID="l1inout" runat="server" CssClass="llet inout inactive"/>
                    <asp:Label ID="l1orario" runat="server" CssClass="llet orario inactive"/>
                    <asp:LinkButton ID="l1delete" runat="server" CssClass="blet" ToolTip="Annulla la registrazione" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnf1" TargetControlID="l1delete" ConfirmText="Confermi l'annullamento della registrazione?" />
                    <asp:LinkButton ID="l1detail" runat="server" CssClass="bletd" ToolTip="Visualizza dettagli nominativo" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <div class="clear">
                    </div>
                </div>
                <div class="lettura">
                    <asp:Label ID="l2name" runat="server" CssClass="llet nome inactive" />
                    <asp:Label ID="l2inout" runat="server" CssClass="llet inout inactive" />
                    <asp:Label ID="l2orario" runat="server" CssClass="llet orario inactive" />
                    <asp:LinkButton ID="l2delete" runat="server" CssClass="blet" ToolTip="Annulla la registrazione" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnf2" TargetControlID="l2delete" ConfirmText="Confermi l'annullamento della registrazione?" />
                    <asp:LinkButton ID="l2detail" runat="server" CssClass="bletd" ToolTip="Visualizza dettagli nominativo" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <div class="clear">
                    </div>
                </div>
                <div class="lettura">
                    <asp:Label ID="l3name" runat="server" CssClass="llet nome inactive" />
                    <asp:Label ID="l3inout" runat="server" CssClass="llet inout inactive" />
                    <asp:Label ID="l3orario" runat="server" CssClass="llet orario inactive" />
                    <asp:LinkButton ID="l3delete" runat="server" CssClass="blet" ToolTip="Annulla la registrazione" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnf3" TargetControlID="l3delete" ConfirmText="Confermi l'annullamento della registrazione?" />
                    <asp:LinkButton ID="l3detail" runat="server" CssClass="bletd" ToolTip="Visualizza dettagli nominativo" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <div class="clear">
                    </div>
                </div>
                <div class="lettura">
                    <asp:Label ID="l4name" runat="server" CssClass="llet nome inactive" />
                    <asp:Label ID="l4inout" runat="server" CssClass="llet inout inactive" />
                    <asp:Label ID="l4orario" runat="server" CssClass="llet orario inactive" />
                    <asp:LinkButton ID="l4delete" runat="server" CssClass="blet" ToolTip="Annulla la registrazione" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnf4" TargetControlID="l4delete" ConfirmText="Confermi l'annullamento della registrazione?" />
                    <asp:LinkButton ID="l4detail" runat="server" CssClass="bletd" ToolTip="Visualizza dettagli nominativo" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <div class="clear">
                    </div>
                </div>
                <div class="lettura">
                    <asp:Label ID="l5name" runat="server" CssClass="llet nome inactive" />
                    <asp:Label ID="l5inout" runat="server" CssClass="llet inout inactive" />
                    <asp:Label ID="l5orario" runat="server" CssClass="llet orario inactive" />
                    <asp:LinkButton ID="l5delete" runat="server" CssClass="blet" ToolTip="Annulla la registrazione" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnf5" TargetControlID="l5delete" ConfirmText="Confermi l'annullamento della registrazione?" />
                     <asp:LinkButton ID="l5detail" runat="server" CssClass="bletd" ToolTip="Visualizza dettagli nominativo" Visible="false">
                        &nbsp;
                    </asp:LinkButton>
                    <div class="clear">
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="stl_dfo titStatistiche">
        <div class="title">
            Statistiche
        </div>
    </div>
    <asp:UpdatePanel ID="updStatistiche" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlStatistiche" runat="server" CssClass="pnlStatistiche">
                <table style="width:100%">
                    <tr>
                        <td class="stLeft">
                            Partecipanti iscritti (non annullati)
                        </td>
                        <td class="stRight">
                            <asp:Label ID="lblIscritti" runat="server" EnableViewState="false" />
                            <a href="javascript:showPersone('TUTTI');">Visualizza</a>
                        </td>
                    </tr>
                    <tr>
                        <td class="stLeft">
                            Partecipanti presenti adesso
                        </td>
                        <td class="stRight">
                            <asp:Label ID="lblPresenti" runat="server" EnableViewState="false" />
                            <a href="javascript:showPersone('PRESENTI');">Visualizza</a>
                        </td>
                    </tr>
                    <tr>
                        <td class="stLeft">
                            Partecipanti non presenti adesso
                        </td>
                        <td class="stRight">
                            <asp:Label ID="lblAssenti" runat="server" EnableViewState="false" />
                            <a href="javascript:showPersone('ASSENTI');">Visualizza</a>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="stl_dfo titCognomeNomeManuale">
        <div class="title">
            Visualizzazione/modifica dati nominativo
        </div>
    </div>
    <asp:UpdatePanel ID="updCognomeNomeManuale" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlCognomeNomeManuale" runat="server" CssClass="pnlCognomeNomeManuale">
                <span class="lblCognomeNome">Nominativo</span>
                <asp:DropDownList ID="ddnCognomeNomeManuale" runat="server" EnableViewState="false" CssClass="ddnCognomeNome" AutoPostBack="true" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
     <div class="stl_dfo titUscitaGenerale">
         <div class="title">
             Registrazione Uscita Generale
         </div>
    </div>
    <asp:UpdatePanel ID="updUscitaGenerale" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlUscitaGenerale" runat="server" CssClass="pnlUscitaGenerale" DefaultButton="btnRegistra">
                <span class="lblCognomeNome">Data</span>
                <asp:TextBox ID="txtUscitaGeneraleData" runat="server" CssClass="txtBarcode" EnableViewState="false" Width="100px" />
                <ajaxToolkit:MaskedEditExtender ID="medDATA" runat="server" TargetControlID="txtUscitaGeneraleData"
                                        Mask="99/99/9999" MaskType="Date" />
                <span class="lblCognomeNome">Orario</span>
                <asp:TextBox ID="txtUscitaGeneraleOra" runat="server" CssClass="txtBarcode" EnableViewState="false" Width="100px" />
                <ajaxToolkit:MaskedEditExtender ID="medINIZIO" runat="server" TargetControlID="txtUscitaGeneraleOra"
                                        Mask="99:99:99" MaskType="Time" />
                <asp:Button ID="btnRegistra" runat="server" Text="Registra" CssClass="btnlink btnBarcode" style="padding:3px 19px 4px 17px;margin:0px;" />
                <ajaxToolkit:ConfirmButtonExtender ID="cnfUscita" runat="server" TargetControlID="btnRegistra" 
                 ConfirmText="Confermi la registrazione dell'uscita generale?" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
