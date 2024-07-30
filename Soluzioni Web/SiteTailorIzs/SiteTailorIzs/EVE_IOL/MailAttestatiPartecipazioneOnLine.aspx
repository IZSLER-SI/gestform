<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="MailAttestatiPartecipazioneOnLine.aspx.vb" Inherits="Softailor.SiteTailorIzs.MailAttestatiPartecipazioneOnLine" %>
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
                Scaricamento attestato di partecipazione attivo per
            </td>
            <td class="mydatatext mybordertop">
                <asp:Label ID="lblListaAttivi" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero <b>partecipanti interni</b> presenti
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumPartecipantiInterni" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero <b>partecipanti esterni</b> presenti
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumPartecipantiEsterni" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero <b>partecipanti interni</b> che hanno superato la verifica di apprendimento
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumPartecipantiInterniTest" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero <b>partecipanti esterni </b> che hanno superato la verifica di apprendimento
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumPartecipantiEsterniTest" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero <b>docenti/relatori/tutor/moderatori interni</b> presenti
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumDRMTInterni" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero <b>docenti/relatori/tutor/moderatori esterni</b> presenti
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumDRMTEsterni" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phdRiepiloghiToDo" runat="server" EnableViewState="false" />
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Invio mail notifica disponibilità attestati di partecipazione on line
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
                        <li>La presenza di tutti i partecipanti / docenti / relatori / moderatori / tutor sia stata correttamente registrata</li>
                        <li>Sia stato attivato lo scaricamento dell'attestato di partecipazione on line per almeno una delle categorie</li>
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
