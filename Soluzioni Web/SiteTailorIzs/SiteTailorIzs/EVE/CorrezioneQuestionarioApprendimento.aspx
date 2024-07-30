<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="CorrezioneQuestionarioApprendimento.aspx.vb" Inherits="Softailor.SiteTailorIzs.CorrezioneQuestionarioApprendimento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <link href="CorrezioneQuestionarioApprendimento.css" rel="stylesheet" />
    <script src="CorrezioneQuestionarioApprendimento.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">
        <asp:Literal ID="ltrTitolo" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <asp:HiddenField ID="hidNumDomande" runat="server" />
    <asp:HiddenField ID="hidMinRisposte" runat="server" />
    <asp:HiddenField ID="hidSeqRisposte" runat="server" />
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="stl_gen_box" style="position:absolute;top:15px;left:15px;width:840px;">
                <div class="content padall" style="height:405px;">
                    <asp:PlaceHolder ID="phdRisposte" runat="server" />
                    <div class="clear"></div>
                </div>
            </div>
            <div class="stl_gen_box" style="position:absolute;top:450px;left:15px;width:840px;">
                <div class="content padall">
                    <div style="padding-bottom:4px;">
                                Numero minimo di risposte esatte per il superamento del questionario: <b><asp:Label runat="server" ID="lbl_MinimoEsatte"></asp:Label></b>
                            </div>
                            <div>
                                <div class="tiporisp">Risposte esatte:</div>
                                <asp:Label ID="lbl_Esatte" runat="server" CssClass="numero esatta" />
                                <div class="tiporisp">Risposte errate:</div>
                                <asp:Label ID="lbl_Errate" runat="server" CssClass="numero errata" />
                                <div class="tiporisp">Risposte non date:</div>
                                <asp:Label ID="lbl_NonDate" runat="server" CssClass="numero nondata" />
                                <div class="tiporisp">Esito:</div>
                                <asp:Label ID="lbl_Esito" runat="server" CssClass="esito" />
                                <div style="float:left">
                                    <asp:LinkButton ID="lnkRicalcola" runat="server" CssClass="btnlink">
                                        Ricalcola
                                    </asp:LinkButton>
                                </div>
                                <div class="clear"></div>
                            </div>
                </div>
            </div>
            <div style="position:absolute;top:515px;left:15px;width:842px;text-align:right;font-size:11px;font-weight:bold;">
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
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
