<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="DefinizioneQuestionarioApprendimento.aspx.vb" Inherits="Softailor.SiteTailorIzs.DefinizioneQuestionarioApprendimento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <link href="DefinizioneQuestionarioApprendimento.css" rel="stylesheet" />
    <script src="DefinizioneQuestionarioApprendimento.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 0px;width:600px;">
        <div class="title">
            Definizione Questionario Valutazione Apprendimento
        </div>
    </div>
    <div class="infoDiv11" style="position: absolute; left: 620px; top: 0px; width: 420px; height:100px;">
        Indica il <b>numero di domande</b> che compongono il questionario, il <b>numero minimo di risposte corrette</b> da dare
        per superarlo e la <b>risposta esatta per ciascuna domanda</b>.<br />
        Per le <b>risposte esatte</b> utilizza una <b>lettera</b> (a,b,c,d...).<br />
        Per salvare clicca su salva, se non desideri salvare clicca su "chiudi senza salvare".<br />
        <b>Attenzione</b>: se modifichi i dati del questionario dopo aver immesso le risposte per uno o più partecipanti,
        tutti i questionari inseriti saranno ricorretti.
    </div>
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 90px;width:600px;">
        <div class="title">
            Risposta esatta per ciascuna domanda
        </div>
    </div>
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="stl_gen_box" style="position:absolute;top:30px;left:0px;width:600px;">
                <div class="content padall">
                    Numero di domande che compongono il questionario
                        <asp:DropDownList ID="ddnNumeroDomande" runat="server" EnableViewState="true" AutoPostBack="true" CssClass="ddn" Font-Bold="true" />
                        <br />
                        Numero minimo di domande corrette per superare il questionario
                        <asp:DropDownList ID="ddnMinimoRisposte" runat="server" EnableViewState="true" CssClass="ddn" Font-Bold="true" Enabled="false" />
                </div>
            </div>
            <div class="stl_gen_box" style="position:absolute;top:120px;left:0px;width:1075px;">
                <div class="content padall" style="height:400px;">
                    <asp:PlaceHolder ID="phdRisposte" runat="server" />
                    <div class="clear"></div>
                </div>
            </div>
            <div style="position:absolute;top:540px;left:0px;width:1077px;text-align:right;font-size:13px;font-weight:bold;">
                <asp:LinkButton ID="lnkSalva" runat="server" CssClass="btnlink">
                    Salva
                </asp:LinkButton>
                &nbsp;
                <ajaxToolkit:ConfirmButtonExtender ID="cnfSalva" runat="server" TargetControlID="lnkSalva" ConfirmText="Confermi il salvataggio?" />
                <asp:LinkButton ID="lnkSalvaChiudi" runat="server" CssClass="btnlink">
                    Salva e chiudi
                </asp:LinkButton>
                <ajaxToolkit:ConfirmButtonExtender ID="cnfSalvaChiudi" runat="server" TargetControlID="lnkSalvaChiudi" ConfirmText="Confermi il salvataggio?" />
                &nbsp;
                <asp:LinkButton ID="lnkChiudi" runat="server" CssClass="btnlink">
                    Chiudi senza salvare
                </asp:LinkButton>
                <ajaxToolkit:ConfirmButtonExtender ID="cnfChiudi" runat="server" TargetControlID="lnkChiudi" ConfirmText="Confermi l'abbandono dei dati inseriti?" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
