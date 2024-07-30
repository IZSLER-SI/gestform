<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="GestioneAttestatiOnLine.aspx.vb" Inherits="Softailor.SiteTailorIzs.GestioneAttestatiOnLine" %>
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
            width: 592px;
            height: 260px;
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
    <asp:UpdatePanel ID="updDati" runat="server">
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
            <table class="mytable">
                <tr>
                    <td class="mylabel">
                        Scaricamento attestati ECM dal sito pubblico
                    </td>
                    <td class="mydatatext">
                        <div>
                            <asp:Label ID="lblStatoAttivazione" runat="server" />
                        </div>
                        <div><br /></div>
                        <div>
                            <asp:LinkButton runat="server" ID="lnkAttiva" Text="Attiva" CssClass="btnlink" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnfAttiva" TargetControlID="lnkAttiva" ConfirmText="Confermi l'attivazione della funzionalità?" />
                            <asp:LinkButton runat="server" ID="lnkDisattiva" Text="Disattiva" CssClass="btnlink" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnfDisattiva" TargetControlID="lnkDisattiva" ConfirmText="Confermi la disattivazione della funzionalità?" />
                        </div>
                        <div><br /></div>
                        <div>
                            <asp:Panel ID="pnlWarningAttiva" runat="server">
                                <b>IMPORTANTE:</b> prima di attivare lo scaricamento degli attestati ECM dal portale pubblico, assicurati che:
                                <ul style="margin-top:0px;margin-bottom:0px;">
                                    <li>Il formato dell'attestato ECM sia stato correttamente impostato</li>
                                    <li>Tutte le operazioni per l'attribuzione delle presenze, la correzione dei questionari, il conferimento dei crediti e la verifica dei dati ECM siano state effettuate</li>
                                </ul>
                            </asp:Panel>
                             <asp:Panel ID="pnlWarningDisattiva" runat="server">
                                <b>IMPORTANTE:</b> se disattivi lo scaricamento degli attestati ECM dal sito pubblico ed hai già inviato le mail di notifica disponibilità, i partecipanti non potranno più scaricare l'attestato pur avendo ricevuto una mail di notifica.
                            </asp:Panel>
                        </div>                        
                    </td>
                </tr>
            </table>
            <asp:PlaceHolder ID="phdRiepiloghi" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
