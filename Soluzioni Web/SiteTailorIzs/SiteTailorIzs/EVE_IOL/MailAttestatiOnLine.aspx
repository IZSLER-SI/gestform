<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="MailAttestatiOnLine.aspx.vb" Inherits="Softailor.SiteTailorIzs.MailAttestatiOnLine" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .mytable
        {
            font-size: 11px;
        }
        .mylabel
        {
            vertical-align: top;
            background-color: #f2f2f2;
            padding: 5px 4px 4px 3px;
            border-bottom: solid 1px #c0c0c0;
            width: 330px;
        }
        .mybordertop
        {
            border-top: solid 1px #c0c0c0;
        }
        .mydata
        {
            vertical-align: top;
            padding: 2px 0px 2px 5px;
            border-bottom: solid 1px #c0c0c0;
            width: 600px;
        }
        .mydatatext
        {
            vertical-align: top;
            padding: 5px 0px 5px 5px;
            border-bottom: solid 1px #c0c0c0;
            width: 600px;
        }
        .mylist
        {
            width: 600px;
            height: 130px;
            overflow-y: scroll;
            border: solid 1px #c0c0c0;
            font-size: 11px;
            line-height: 15px;
            padding: 4px;
        }
        .mya
        {
            font-weight: bold;
            color: #336699;
            text-decoration: none;
        }
        .mya:hover
        {
            color: #ff6600;
        }
        .mymail
        {
            color: #555555;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
    <ContentTemplate>
    <table class="mytable">
        <tr>
            <td class="mylabel mybordertop">
                Numero <b>partecipanti</b> che hanno diritto ai crediti ECM
            </td>
            <td class="mydatatext mybordertop">
                <asp:Label ID="lblNumPartecipanti" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero <b>docenti/relatori/tutor/moderatori</b> che hanno diritto ai crediti ECM
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumRelatori" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phdRiepiloghiToDo" runat="server" EnableViewState="false" />
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Invio mail notifica disponibilità attestati ECM on line
            </td>
            <td class="mydatatext">
                <div>
                    <asp:Label ID="lblStatoInvio" runat="server" />
                </div>
                <asp:Panel ID="pnlSpacer1" runat="server">
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlWarningInvia" runat="server">
                    <b>IMPORTANTE:</b> prima di effettuare l'invio delle mail, assicurati che:
                    <ul style="margin-top: 0px; margin-bottom: 0px;">
                        <li>Il formato dell'attestato ECM sia stato correttamente impostato</li>
                        <li>Tutte le operazioni per l'attribuzione delle presenze, la correzione dei questionari,
                            il conferimento dei crediti e la verifica dei dati ECM siano state effettuate</li>
                        <li>Sia stato attivato lo scaricamento dell'attestato ECM on line</li>
                    </ul>
                </asp:Panel>
                <asp:Panel ID="pnlSpacer2" runat="server">
                    <br />
                </asp:Panel>
                <div>
                    <asp:Button runat="server" ID="lnkInvia" Text="Invia Mail" CssClass="btnlink" />
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnfInvia" TargetControlID="lnkInvia"
                        ConfirmText="Confermi l'invio delle mail?" />
                </div>
                <asp:Panel ID="pnlSpacer3" runat="server">
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlWarning2invia" runat="server">
                    <b>ATTENZIONE:</b> L'invio delle mail può richiedere parecchio tempo (anche qualche
                    minuto). Fai clic una volta sola sul pulsante "Invia Mail" e attendi il termine dell'operazione.
                </asp:Panel>
                <asp:Panel ID="pnlSpacer4" runat="server">
                    <br />
                </asp:Panel>
                <div>
                    <asp:Label ID="lblRisultatoInvio" runat="server" EnableViewState="false" />
                </div>
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phdRiepiloghiDone" runat="server" EnableViewState="false" />
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
