<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="CriteriAccesso.aspx.vb" Inherits="Softailor.SiteTailorIzs.CriteriAccesso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <link href="CriteriAccesso.css" rel="stylesheet" />
    <script src="CriteriAccesso.js"></script>
     <script type="text/javascript">
         <asp:Literal ID="ltrRepositioning" runat="server" />    
    </script>
    <div style="display:none;">
        <asp:UpdatePanel ID="updHiddenCtls" runat="server" EnableViewState="true" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- controlli nascosti -->
                <asp:LinkButton ID="lnkRefreshCriteri" runat="server">-</asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="updDati" runat="server" EnableViewState="false" UpdateMode="Always">
        <ContentTemplate>
            <div class="stl_dfo" style="width:720px;">
                <div class="title">
                    Dati base per iscrizioni on line
                </div>
                <table class="fieldtable">
                    <tr>
                        <td class="labelcol bordertop">Data inizio visibilità evento su portale pubblico</td>
                        <td class="datacol bordertop" style="width:200px;">
                            <asp:TextBox ID="iol_dt_INIZIOVISIBILITA" runat="server" CssClass="txt txtdate" />
                            <ajaxToolkit:MaskedEditExtender ID="mediol_dt_INIZIOVISIBILITA" runat="server" TargetControlID="iol_dt_INIZIOVISIBILITA"
                                Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                        </td>
                        <td class="errorcol bordertop">
                            <asp:Label ID="erriol_dt_INIZIOVISIBILITA" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">
                            Data apertura iscrizioni on line
                        </td>
                        <td class="datacol" style="width:200px;">
                            <asp:TextBox ID="iol_dt_APERTURAISCRIZIONI" runat="server" CssClass="txt txtdate" />
                            <ajaxToolkit:MaskedEditExtender ID="mediol_dt_APERTURAISCRIZIONI" runat="server" TargetControlID="iol_dt_APERTURAISCRIZIONI"
                                 Mask="99/99/9999" MaskType="Date" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="erriol_dt_APERTURAISCRIZIONI" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">
                            Data chiusura iscrizioni on line
                        </td>
                        <td class="datacol" style="width:200px;">
                            <asp:TextBox ID="iol_dt_CHIUSURAISCRIZIONI" runat="server" CssClass="txt txtdate" />
                            <ajaxToolkit:MaskedEditExtender ID="mediol_dt_CHIUSURAISCRIZIONI" runat="server" TargetControlID="iol_dt_CHIUSURAISCRIZIONI"
                                Mask="99/99/9999" MaskType="Date" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="erriol_dt_CHIUSURAISCRIZIONI" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Numero massimo di partecipanti (vuoto=nessun limite)</td>
                        <td class="datacol" style="width:200px;">
                            <asp:TextBox ID="iol_ni_MAXPARTECIPANTI" runat="server" CssClass="txt txtnarrow" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="erriol_ni_MAXPARTECIPANTI" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                </table>
                <div style="padding-top:10px;padding-bottom:20px;font-size:15px;font-weight:bold;font-family:Arial;">
                    <asp:LinkButton ID="lnkSaveHeader" runat="server" CssClass="btnlink" Text="Salva" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updCriteri" runat="server" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>
             <div class="stl_dfo" style="width:1200px;">
                <div class="title">
                    Criteri per l'accesso all'evento
                </div>
            </div>
            <asp:PlaceHolder ID="phdCriteri" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
