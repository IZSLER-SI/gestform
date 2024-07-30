<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="MailChiusuraListe.aspx.vb" Inherits="Softailor.SiteTailorIzs.MailChiusuraListe" %>
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
        .mygroup
        {
            font-weight:bold;
            color:#ff6600;
        }
        .myitem
        {
            padding-left:15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
<asp:UpdatePanel ID="updContent" runat="server">
    <ContentTemplate>
    <table border="0" class="mytable">
        <tr>
            <td class="mylabel mybordertop">
                Numero partecipanti <b>iscritti con conferma immediata</b>
            </td>
            <td class="mydatatext mybordertop">
                <asp:Label ID="lblNumIscrittiConfermaImmediata" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table border="0" class="mytable">
        <tr>
            <td class="mylabel">
                Numero partecipanti <b>accettati in fase di chiusura liste</b>
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumAccettatiChiusuraListe" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero partecipanti <b>non accettati</b>
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumNonAccettati" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Numero <b>docenti / relatori / moderatori</b>
            </td>
            <td class="mydatatext">
                <asp:Label ID="lblNumDRM" runat="server" Font-Bold="true" />
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phdRiepiloghiToDo" runat="server" EnableViewState="false" />
    <table class="mytable">
        <tr>
            <td class="mylabel">
                Invio mail chiusura liste d'attesa
            </td>
            <td class="mydatatext">
                <div>
                    <asp:Label ID="lblRisultatoInvio" runat="server" EnableViewState="false" />
                </div>
                <div>
                    <asp:Label ID="lblStatoInvio" runat="server" />
                </div>
                <asp:Panel ID="pnlSpacer1" runat="server"></asp:Panel>
                <asp:Panel ID="pnlWarningInvia" runat="server">
                    <b>IMPORTANTE:</b> prima di effettuare l'invio delle mail, assicurati di aver effettuato l'elaborazione delle liste d'attesa.
                </asp:Panel>
                <asp:Panel ID="pnlSpacer2" runat="server">
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlPrimoInvio" runat="server">
                    <div style="border-left:5px solid #c0c0c0;padding-left:10px;">
                        <div><asp:CheckBox ID="chkInviaA" runat="server" Text="Promemoria partecipazione discenti" Checked="true" /></div>
                        <div><asp:CheckBox ID="chkInviaB" runat="server" Text="Notifica accettazione discenti" Checked="true" /></div>
                        <div><asp:CheckBox ID="chkInviaC" runat="server" Text="Notifica mancata accettazione discenti" Checked="true" /></div>
                        <div style="padding-bottom:5px;"><asp:CheckBox ID="chkInviaD" runat="server" Text="Promemoria partecipazione docenti / relatori / moderatori" Checked="true" /></div>
                    
                        <asp:Button runat="server" ID="lnkInvia" CssClass="btn btnlink" Text="Invia le mail delle tipologie selezionate" />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnfInvia" TargetControlID="lnkInvia"
                            ConfirmText="Confermi l'invio delle mail delle tipologie selezionate?" />
                    </div>
                </asp:Panel>
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

                
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phdRiepiloghiDone" runat="server" EnableViewState="false" />
    
    <table class="mytable" runat="server" id="tblReinvio">
        <tr>
            <td class="mylabel">
                Invio mail ai nominativi ai quali non è stata inviata la mail
            </td>
            <td class="mydatatext">
                <div style="border-left:5px solid #c0c0c0;padding-left:10px;">
                    <div><asp:CheckBox ID="chkReInviaA" runat="server" Text="Promemoria partecipazione discenti" Checked="true" /></div>
                    <div><asp:CheckBox ID="chkReInviaB" runat="server" Text="Notifica accettazione discenti" Checked="true" /></div>
                    <div><asp:CheckBox ID="chkReInviaC" runat="server" Text="Notifica mancata accettazione discenti" Checked="true" /></div>
                    <div style="padding-bottom:5px;"><asp:CheckBox ID="chkReInviaD" runat="server" Text="Promemoria partecipazione docenti / relatori / moderatori" Checked="true" /></div>
                    
                    <asp:Button runat="server" ID="lnkReinvia" CssClass="btn btnlink" Text="Invia le mail delle tipologie selezionate" />
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnfReinvia" TargetControlID="lnkReinvia"
                        ConfirmText="Confermi l'invio delle mail delle tipologie selezionate?" />
                </div>
            </td>
        </tr>
    </table>
        </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
