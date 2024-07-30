<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="GestioneAttestatiPartecipazioneOnLine.aspx.vb" Inherits="Softailor.SiteTailorIzs.GestioneAttestatiPartecipazioneOnLine" %>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkAnteprimaPartecipante" />
            <asp:PostBackTrigger ControlID="lnkAnteprimaDRMT" />
        </Triggers>
        <ContentTemplate>
            <table class="mytable">
                <tr>
                    <td class="mylabel mybordertop">
                        Anteprime
                    </td>
                    <td class="mydatatext mybordertop">
                        <asp:LinkButton runat="server" ID="lnkAnteprimaPartecipante" Text="Partecipante" CssClass="btnlink" />
                        &nbsp;
                        <asp:LinkButton runat="server" ID="lnkAnteprimaDRMT" Text="Docente / Tutor / Moderatore" CssClass="btnlink" />
                    </td>
                </tr>
            </table>
            <table class="mytable">
                <tr>
                    <td class="mylabel">
                        Testo partecipazione
                    </td>
                    <td class="mydatatext">
                        <asp:TextBox ID="txttx_ATTPART_HAPART" runat="server" EnableViewState="false" MaxLength="500" CssClass="txt" Width="590px" />
                        <br />
                        <asp:LinkButton runat="server" ID="lnkSavetxttx_ATTPART_HAPART" CssClass="btnlink">Salva Testo</asp:LinkButton>
                        <br />
                        Questo testo, se inserito, sostituisce la dicitura "ha partecipato all'evento formativo" negli attestati dei partecipanti.
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
                        Numero <b>docenti/relatori/moderatori/tutor interni</b> presenti
                    </td>
                    <td class="mydatatext">
                        <asp:Label ID="lblNumDRMTInterni" runat="server" Font-Bold="true" />
                    </td>
                </tr>
            </table>
            <table class="mytable">
                <tr>
                    <td class="mylabel">
                        Numero <b>docenti/relatori/moderatori/tutor esterni</b> presenti
                    </td>
                    <td class="mydatatext">
                        <asp:Label ID="lblNumDRMTEsterni" runat="server" Font-Bold="true" />
                    </td>
                </tr>
            </table>
            <table class="mytable">
                <tr>
                    <td class="mylabel">
                        Attivazione scaricamento da sito pubblico
                    </td>
                    <td class="mydatatext">
                        <div>
                            <asp:CheckBox ID="chkPI" runat="server" EnableViewState="false" Text="Partecipanti Interni Presenti" />
                        </div>
                        <div>
                            <asp:CheckBox ID="chkPE" runat="server" EnableViewState="false" Text="Partecipanti Esterni Presenti" />
                        </div>
                        <div>
                            <asp:CheckBox ID="chkPIT" runat="server" EnableViewState="false" Text="Partecipanti Interni Che Hanno Superato La Verifica di Apprendimento" />
                        </div>
                        <div>
                            <asp:CheckBox ID="chkPET" runat="server" EnableViewState="false" Text="Partecipanti Esterni Che Hanno Superato La Verifica di Apprendimento" />
                        </div>
                        <div>
                            <asp:CheckBox ID="chkDRMTI" runat="server" EnableViewState="false" Text="Docenti/Relatori/Moderatori/Tutor Interni Presenti" />
                        </div>
                        <div>
                            <asp:CheckBox ID="chkDRMTE" runat="server" EnableViewState="false" Text="Docenti/Relatori/Moderatori/Tutor Esterni Presenti" />
                        </div>
                        <div>
                            <asp:LinkButton runat="server" ID="lnkSalva" Text="Salva Impostazioni" CssClass="btnlink" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnfSalva" TargetControlID="lnkSalva" ConfirmText="Confermi l'operazione?" />
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
